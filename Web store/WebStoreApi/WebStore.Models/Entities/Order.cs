using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class Order : EntityBase<int>
    {
        public Order()
        {
            CartItems = new HashSet<CartItem>();
            HistoryRecords = new HashSet<OrderHistory>();
        }

        public decimal TotalPrice { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderHistory> HistoryRecords { get; set; }
    }
}
