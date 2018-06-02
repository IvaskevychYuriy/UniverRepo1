using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;

namespace WebStoreApi.Jobs
{
    public class OrdersStateUpdateJob
    {
        private readonly ApplicationDbContext _dbContext;

        public OrdersStateUpdateJob(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task UpdateOrderStates()
        {
            var doneIds = await _dbContext.Orders
                .Where(o => !o.HistoryRecords.Any(h => h.State == OrderStates.Done))
                .SelectMany(o => o.CartItems)
                .Where(x => x.StorageItem == null)
                .Select(x => x.OrderId)
                .Distinct()
                .ToListAsync();

            var itemsToUpdate = await _dbContext.Orders
                .Where(o => !doneIds.Contains(o.Id) && !o.HistoryRecords.Any(h => h.State == OrderStates.Done))
                .ToListAsync();

            var utcNow = DateTime.UtcNow;
            foreach (var order in itemsToUpdate)
            {
                order.HistoryRecords.Add(new OrderHistory()
                {
                    State = OrderStates.Done,
                    StateChangeDate = utcNow
                });
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
