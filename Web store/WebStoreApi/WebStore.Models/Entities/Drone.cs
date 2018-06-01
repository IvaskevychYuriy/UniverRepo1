using System;
using WebStore.Models.Enumerations;

namespace WebStore.Models.Entities
{
    public class Drone : EntityBase<int>
    {
        public DroneStates State { get; set; }
        public DateTime? ArrivalTime { get; set; } // when it becomes available again

        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }

        public int? CartItemId { get; set; }
        public virtual CartItem CartItem { get; set; }
    }
}
