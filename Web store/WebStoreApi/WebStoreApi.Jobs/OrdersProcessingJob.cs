using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;
using WebStoreApi.Jobs.Helpers;
using WebStoreApi.Jobs.Models;
using WebStoreApi.Logic.Algorithms;

namespace WebStoreApi.Jobs
{
    public class OrdersProcessingJob
    {
        private readonly ApplicationDbContext _dbContext;

        public OrdersProcessingJob(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ProcessOrders()
        {
            var info = await FetchInfo();
            if (!info.Orders.Any() || !info.Storages.Any())
            {
                return;
            }

            var problemData = ProblemDataHelper.Generate(info);
            var result = CalculateResult(problemData);
            await ShipProducts(info, problemData, result);
        }

        private int[] CalculateResult(ProblemData problemData)
        {
            var result = new int[problemData.StoragesCount * problemData.ProductsCount * problemData.OrdersCount];
            var simplex = new Simplex(problemData.Data);

            simplex.Calculate(result);
            return result;
        }

        // ships result[index] amount of j-th product from i-th storage for k-th order
        private async Task ShipProducts(OrdersProcessingInfo info, ProblemData data, int[] result)
        {
            var orderIds = info.Orders.Select(o => o.OrderId).ToList();
            var orders = await _dbContext.Orders
                .Where(o => orderIds.Contains(o.Id))
                .Select(o => new
                {
                    Id = o.Id,
                    HistoryRecords = o.HistoryRecords.ToList(),
                    CartItems = o.CartItems
                        .Where(ci => ci.StorageItem == null)
                        .ToList()
                })
                .ToListAsync();

            var storageIds = info.Storages.Select(s => s.StorageId).ToList();
            var storages = await _dbContext.Storages
                .Where(s => storageIds.Contains(s.Id))
                .Select(s => new
                {
                    Storage = s,
                    Items = s.Items
                        .Where(i => i.CartItemId == null)
                        .ToList(),
                    Drones = s.Drones
                        .Where(d => d.State == DroneStates.Available)
                        .ToList()
                })
                .ToListAsync();

            var utcNow = DateTime.UtcNow;

            for (int k = 0; k < data.OrdersCount; k++)
            {
                if (!orders[k].HistoryRecords.Any(h => h.State == OrderStates.Processing))
                {
                    await _dbContext.OrderHistories.AddAsync(new OrderHistory()
                    {
                        OrderId = orders[k].Id,
                        State = OrderStates.Processing,
                        StateChangeDate = utcNow
                    });
                }
            }

            for (int i = 0; i < data.StoragesCount; i++)
            {
                for (int j = 0; j < data.ProductsCount; j++)
                {
                    for (int k = 0; k < data.OrdersCount; k++)
                    {
                        int index = i * data.ProductsCount * data.OrdersCount + j * data.OrdersCount + k;
                        if (result[index] <= 0)
                        {
                            continue;
                        }

                        var arrivalTime = utcNow.AddSeconds(info.Orders[k].Coordinates.Distance(info.Storages[i].Coordinates) * 2); // 1 km/s twice the distance
                        var cartItems = orders[k].CartItems
                            .Where(ci => ci.ProductId == info.ProductIds[j] && ci.Drone == null && ci.StorageItem == null)
                            .ToList();
                        var drones = storages[i].Drones
                            .Where(d => d.CartItemId == null)
                            .ToList();
                        var items = storages[i].Items
                            .Where(it => it.ProductId == info.ProductIds[j] && it.CartItemId == null)
                            .ToList();
                        
                        for (int l = 0; l < result[index] && l < cartItems.Count && l < drones.Count && l < items.Count; ++l)
                        {
                            _dbContext.Entry(drones[l]).State = EntityState.Modified;
                            _dbContext.Entry(cartItems[l]).State = EntityState.Modified;
                            _dbContext.Entry(items[l]).State = EntityState.Modified;

                            drones[l].ArrivalTime = arrivalTime;
                            drones[l].State = DroneStates.Busy;
                            drones[l].CartItemId = cartItems[l].Id;

                            cartItems[l].Drone = drones[l];
                            cartItems[l].StorageItem = items[l];

                            items[l].CartItemId = cartItems[l].Id;
                            items[l].State = StorageItemStates.Ordered;
                        }
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task<OrdersProcessingInfo> FetchInfo()
        {
            var ordersData = await _dbContext.Orders
                .AsNoTracking()
                .Where(o => o.HistoryRecords.Any(h => h.State != OrderStates.Done) && o.CartItems.Any(ci => ci.StorageItem == null))
                .Select(o => new
                {
                    OrderId = o.Id,
                    Coordinates = o.Coordinates,
                    ProductCounts = o.CartItems
                        .GroupBy(ci => ci.ProductId)
                        .Select(g => new
                        {
                            ProductId = g.Key,
                            Count = g.Count()
                        })
                        .ToList()
                })
                .ToListAsync();

            var distinctProductIds = ordersData
                .SelectMany(o => o.ProductCounts.Select(x => x.ProductId))
                .Distinct()
                .OrderBy(pid => pid)
                .ToList();

            var ordersInfo = ordersData
                .Select(o => new OrderInfo()
                {
                    OrderId = o.OrderId,
                    Coordinates = o.Coordinates,
                    ProductCounts = distinctProductIds
                        .Select(pid => o.ProductCounts.FirstOrDefault(x => x.ProductId == pid)?.Count ?? 0)
                        .ToList()
                })
                .ToList();

            var storagesData = await _dbContext.Storages
                .AsNoTracking()
                .Select(s => new
                {
                    StorageId = s.Id,
                    Coordinates = s.Coordinates,
                    DronesCount = s.Drones.Count(d => d.State == DroneStates.Available),
                    ProductCounts = s.Items
                        .Where(si => distinctProductIds.Contains(si.ProductId))
                        .GroupBy(si => si.ProductId)
                        .Select(g => new
                        {
                            ProductId = g.Key,
                            Count = g.Count()
                        })
                        .ToList()
                })
                .ToListAsync();

            var storagesInfo = storagesData
                .Where(p => p.DronesCount > 0 && p.ProductCounts.Any())
                .Select(o => new StorageInfo()
                {
                    StorageId = o.StorageId,
                    Coordinates = o.Coordinates,
                    DronesCount = o.DronesCount,
                    ProductCounts = distinctProductIds
                            .Select(pid => o.ProductCounts.FirstOrDefault(x => x.ProductId == pid)?.Count ?? 0)
                            .ToList()
                })
                .ToList();

            return new OrdersProcessingInfo()
            {
                Orders = ordersInfo,
                Storages = storagesInfo,
                ProductIds = distinctProductIds
            };
        }
    }
}
