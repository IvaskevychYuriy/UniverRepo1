using System;
using System.Linq;
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
        public void UpdateDronesStates()
        {
            var drones = _dbContext.Drones
                .Where(d => d.State == DroneStates.Busy && d.ArrivalTime != null && d.ArrivalTime < DateTime.UtcNow)
                .ToList();
            
            foreach (var drone in drones)
            {
                drone.ArrivalTime = null;
                drone.CartItemId = null;
                drone.State = DroneStates.Available;
            }

            _dbContext.SaveChanges();
        }
    }
}
