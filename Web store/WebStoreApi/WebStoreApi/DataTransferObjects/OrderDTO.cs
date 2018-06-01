using System.Collections.Generic;
using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class OrderDTO : IDataTransferObject
    {
        public OrderDTO()
        {
            CartItems = new List<CartItemDTO>();
        }

        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public AddressCoordinatesDTO Coordinates { get; set; }

        public List<CartItemDTO> CartItems { get; set; }
        public List<OrderHistoryDTO> HistoryRecords { get; set; }
    }
}
