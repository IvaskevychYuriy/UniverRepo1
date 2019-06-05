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
			var result = await Task.Run(() => BinPacker.Pack(input));
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
					Drones = s.Drones
						.Where(d => d.State == DroneStates.Available)
						.Select(d => new DroneData()
						{
							DroneId = d.Id,
							MaxWeight = d.MaxWeight
						})
						.ToList()
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
					Priority = o.ProcessingPriority,
					Coordinates = o.Coordinates,
					Products = o.CartItems
						.Where(c => c.StorageItem == null)
						.Select(ci => new CartItemData()
						{
							CartItemId = ci.Id,
							Weight = ci.Product.Weight
						})
						.ToList()
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
					Priority = o.Priority,
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
		
		private async Task ShipProducts(ProcessingData data, BinPackerResult result)
		{
			var dronesIds = result.Bins.Select(b => b.Id).ToList();
			var ordersIds = result.Bins.Select(b => b.ItemSet.Id).ToList();
			var cartItemsIds = result.Bins.SelectMany(b => b.ItemSet.Items.Select(i => i.Id)).ToList();

			var storage = await _dbContext.Storages
				.Include(s => s.Items)
				.FirstAsync(s => s.Id == data.Storage.StorageId);

			var storageItems = storage.Items
				.Where(i => i.CartItemId == null)
				.ToList();

			var drones = await _dbContext.Drones
				.Where(d => d.StorageId == data.Storage.StorageId && dronesIds.Contains(d.Id))
				.ToListAsync();

			var orders = await _dbContext.Orders
				.Include(o => o.HistoryRecords)
				.Include(o => o.CartItems)
				.Where(o => ordersIds.Contains(o.Id))
				.ToListAsync();

			var utcNow = DateTime.UtcNow;
			foreach (var bin in result.Bins)
			{
				var drone = drones.First(d => d.Id == bin.Id);
				var order = orders.First(o => o.Id == bin.ItemSet.Id);

				var arrivalTime = utcNow.AddSeconds(order.Coordinates.Distance(storage.Coordinates) * 2); // 1 km/s twice the distance
				drone.ArrivalTime = arrivalTime;
				drone.State = DroneStates.Busy;

				if (!order.HistoryRecords.Any(h => h.State == OrderStates.Processing))
				{
					order.HistoryRecords.Add(new OrderHistory()
					{
						State = OrderStates.Processing,
						StateChangeDate = utcNow
					});
				}

				foreach (var item in bin.ItemSet.Items)
				{
					var cartItem = order.CartItems.First(i => i.Id == item.Id);
					var storageItem = storageItems.FirstOrDefault(i =>i.CartItemId == null && i.ProductId == cartItem.ProductId);
					if (storageItem != null)
					{
						cartItem.DroneId = drone.Id;
						storageItem.CartItemId = cartItem.Id;
						storageItem.State = StorageItemStates.Ordered;
						order.ProcessingPriority = 0;
					}
				}
			}

			var notShippedOrdersIds = data.Orders
				.Select(o => o.OrderId)
				.Except(ordersIds)
				.ToList();

			if (notShippedOrdersIds.Any())
			{
				var notShippedOrders = await _dbContext.Orders
					.Where(o => notShippedOrdersIds.Contains(o.Id))
					.ToListAsync();
				foreach (var order in notShippedOrders)
				{
					order.ProcessingPriority += 1;
				}
			}

			await _dbContext.SaveChangesAsync();
		}
    }
}
