using System;
using WebStore.Api.Contracts;
using WebStore.Models.Enumerations;

namespace WebStore.Api.DataTransferObjects
{
    public class OrderHistoryDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public OrderStates State { get; set; }
        public DateTime StateChangeDate { get; set; }
    }
}
