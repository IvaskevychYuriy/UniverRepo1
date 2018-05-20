namespace WebStore.Models.Entities
{
    public class StorageItem : EntityBase<int>
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }

        public int ProductId { get; set; }
        public virtual ProductItem ProductItem { get; set; }
    }
}
