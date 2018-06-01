using System.Collections.Generic;
using WebStore.Models.Models;

namespace WebStore.Models.Entities
{
    public class Storage : EntityBase<int>
    {
        public Storage()
        {
            Items = new HashSet<StorageItem>();
            Drones = new HashSet<Drone>();
        }

        public string Name { get; set; }

        public AddressCoordinates Coordinates { get; set; }

        public ICollection<StorageItem> Items { get; set; }

        public ICollection<Drone> Drones { get; set; }
    }
}
