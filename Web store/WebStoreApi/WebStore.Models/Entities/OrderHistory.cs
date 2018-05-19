using System;
using WebStore.Models.Enumerations;

namespace WebStore.Models.Entities
{
    public class OrderHistory : EntityBase<int>
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        public OrderStates State { get; set; }
        public DateTime StateChangeDate { get; set; }
    }
}
