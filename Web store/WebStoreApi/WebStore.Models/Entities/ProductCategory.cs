using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class ProductCategory : EntityBase<int>
    {
        public string Name { get; set; }

        public List<ProductItem> Products { get; set; }
    }
}