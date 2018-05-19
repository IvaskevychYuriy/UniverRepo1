using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class ProductSubCategory : EntityBase<int>
    {
        public ProductSubCategory()
        {
            Products = new HashSet<ProductItem>();
        }

        public string Name { get; set; }
        
        public int CategoryId { get; set; }
        public virtual ProductCategory Category { get; set; }

        public ICollection<ProductItem> Products { get; set; }
    }
}
