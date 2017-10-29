using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class ProductItem : EntityBase<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
