using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class ProductItem : EntityBase<int>
    {
        public ProductItem()
        {
            CartItems = new HashSet<CartItem>();
            StorageItems = new HashSet<StorageItem>();
        }

        public bool Active { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        
        public int SubCategoryId { get; set; }
        public virtual ProductSubCategory SubCategory { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public ICollection<StorageItem> StorageItems { get; set; }
    }
}
