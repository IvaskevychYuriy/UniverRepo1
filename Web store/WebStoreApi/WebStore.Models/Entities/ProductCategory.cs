using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class ProductCategory : EntityBase<int>
    {
        public ProductCategory()
        {
            SubCategories = new HashSet<ProductSubCategory>();
        }

        public string Name { get; set; }

        public ICollection<ProductSubCategory> SubCategories { get; set; }
    }
}