using System.Collections.Generic;
using WebStore.Models.Enumerations;

namespace WebStore.Api.DataTransferObjects
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStates State { get; set; }

        public List<CartItemDTO> CartItems { get; set; }
    }
}
