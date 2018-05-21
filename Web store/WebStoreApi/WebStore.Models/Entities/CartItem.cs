namespace WebStore.Models.Entities
{
    public class CartItem : EntityBase<int>
    {
        public decimal ProductPrice { get; set; }

        public int ProductId { get; set; }
        public virtual ProductItem Product { get; set; }
        
        public virtual StorageItem StorageItem { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}