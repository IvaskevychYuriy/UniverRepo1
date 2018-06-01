using System;
using WebStore.Api.Contracts;
using WebStore.Models.Enumerations;

namespace WebStore.Api.DataTransferObjects
{
    public class DroneDTO : IDataTransferObject
    {
        public int Id { get; set; }

        public DroneStates State { get; set; }
        public DateTime? ArrivalTime { get; set; }
        
        public int? CartItemId { get; set; }
    }
}
