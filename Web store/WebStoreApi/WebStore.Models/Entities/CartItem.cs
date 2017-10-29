namespace WebStore.Models.Entities
{
    public class CartItem : EntityBase<int>
    {
        public int ProductId { get; set; }
        public ProductItem Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }
    }
}