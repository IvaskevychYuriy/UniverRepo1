namespace WebStore.Models.Entities
{
    public class CartItem : EntityBase<int>
    {
        public int ProductId { get; set; }
        public virtual ProductItem Product { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int Quantity { get; set; }
    }
}