using System.Collections.Generic;
using WebStore.Models.Enumerations;

namespace WebStore.Models.Entities
{
    public class Order : EntityBase<int>
    {
        public decimal TotalPrice { get; set; }
        public OrderStates State { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
