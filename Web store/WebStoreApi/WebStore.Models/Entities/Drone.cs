using System;
using System.Collections.Generic;
using WebStore.Models.Enumerations;

namespace WebStore.Models.Entities
{
    public class Drone : EntityBase<int>
    {
        public DroneStates State { get; set; }
        public DateTime? ArrivalTime { get; set; } // when it becomes available again
		public float MaxWeight { get; set; }

        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }
		
        public virtual List<CartItem> CartItems { get; set; }
    }
}
