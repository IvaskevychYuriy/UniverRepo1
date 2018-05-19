using System.Collections.Generic;

namespace WebStore.Api.DataTransferObjects
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }

        public List<CartItemDTO> CartItems { get; set; }
        public List<OrderHistoryDTO> HistoryRecords { get; set; }
    }
}
