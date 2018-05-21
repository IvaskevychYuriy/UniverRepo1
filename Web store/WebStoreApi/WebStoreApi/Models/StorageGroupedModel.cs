using System.Collections.Generic;
using WebStore.Models.Entities;

namespace WebStore.Api.Models
{
    public class StorageGroupedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StorageGroupedItemModel> Items { get; set; }
    }

    public class StorageGroupedItemModel
    {
        public int ProductId { get; set; }
        public ProductItem Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
