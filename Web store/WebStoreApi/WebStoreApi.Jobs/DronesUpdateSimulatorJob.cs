using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contexts;
using WebStore.Models.Enumerations;

namespace WebStoreApi.Jobs
{
    /// <summary>
    ///     Simulates the update of arrival time and state of drones
    /// </summary>
    public class DronesUpdateSimulatorJob
    {
        private readonly ApplicationDbContext _dbContext;

        public DronesUpdateSimulatorJob(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        ///     Resets state and arrival date for already arrived drones
        /// </summary>
        public async Task UpdateDronesStates()
        {
            var drones = await _dbContext.Drones
                .Include(d => d.CartItems)
                .ThenInclude(ci => ci.StorageItem)
                .Where(d => d.State == DroneStates.Busy && d.CartItems.Any() && d.ArrivalTime != null && d.ArrivalTime < DateTime.UtcNow)
                .ToListAsync();
            
            foreach (var drone in drones)
            {
                drone.ArrivalTime = null;
                drone.State = DroneStates.Available;
				foreach (var item in drone.CartItems)
				{
					item.StorageItem.State = StorageItemStates.Shipped;
					item.DroneId = null;
				}
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
