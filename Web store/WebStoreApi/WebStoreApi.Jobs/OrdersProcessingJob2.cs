using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;
using WebStoreApi.Jobs.Models;
using WebStoreApi.Logic.BinPacker;
using WebStoreApi.Logic.BinPacker.Models;

namespace WebStoreApi.Jobs
{
	public class OrdersProcessingJob2
    {
        private readonly ApplicationDbContext _dbContext;

        public OrdersProcessingJob2(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ProcessOrders()
        {
			var data = await GetProcessingDataAsync();
			var input = MapToAlgoData(data);
			var result = BinPacker.Pack(input);
			await ShipProducts(data, result);
        }

		private async Task<ProcessingData> GetProcessingDataAsync()
		{
			var data = new ProcessingData();

			data.Storage = await GetStorageDataAsync();
			data.Orders = await GetOrdersDataAsync();

			return data;
		}

		private Task<StorageData> GetStorageDataAsync()
		{
			return _dbContext.Storages
				.AsNoTracking()
				.Select(s => new StorageData()
				{
					StorageId = s.Id,
					Coordinates = s.Coordinates,
					Drones = s.Drones.Select(d => new DroneData()
					{
						DroneId = d.Id,
						MaxWeight = d.MaxWeight
					}).ToList()
				})
				.FirstAsync();
		}

		private Task<List<OrderData>> GetOrdersDataAsync()
		{
			return _dbContext.Orders
				.AsNoTracking()
				.Where(o => !o.HistoryRecords.Any(h => h.State == OrderStates.Done) && o.CartItems.Any(ci => ci.StorageItem == null))
				.Select(o => new OrderData()
				{
					OrderId = o.Id,
					Coordinates = o.Coordinates,
					Products = o.CartItems.Select(ci => new CartItemData()
					{
						CartItemId = ci.Id,
						Weight = ci.Product.Weight
					}).ToList()
				})
				.ToListAsync();
		}
		
		private BinPackerInput MapToAlgoData(ProcessingData data)
		{
			var result = new BinPackerInput();

			result.Bins = data.Storage.Drones
				.Select(d => new Bin()
				{
					Id = d.DroneId,
					Capacity = d.MaxWeight
				})
				.ToList();

			result.ItemSets = data.Orders
				.Select(o => new ItemCollection()
				{
					Id = o.OrderId,
					Items = o.Products
						.Select(p => new Item()
						{
							Id = p.CartItemId,
							Weight = p.Weight
						})
						.ToList()
				})
				.ToList();

			return result;
		}
		
		private Task ShipProducts(ProcessingData data, BinPackerResult result)
		{
			throw new NotImplementedException();
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
    }
}
