using System.Collections.Generic;
using WebStore.Models.Entities;
using WebStore.Models.Models;

namespace WebStore.Api.Models
{
    public class StorageGroupedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressCoordinates Coordinates { get; set; }
        public List<StorageGroupedItemModel> Items { get; set; }
        public List<Drone> Drones { get; set; }
    }

    public class StorageGroupedItemModel
    {
        public int ProductId { get; set; }
        public int AvailableCount { get; set; }
        public ProductItem Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }
	}
}
