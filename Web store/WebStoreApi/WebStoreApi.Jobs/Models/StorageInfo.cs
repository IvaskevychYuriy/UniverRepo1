using System.Collections.Generic;
using WebStore.Models.Models;

namespace WebStoreApi.Jobs.Models
{
    public class StorageInfo
    {
        public int StorageId { get; set; }

        public AddressCoordinates Coordinates { get; set; }

        public int DronesCount { get; set; }

        public List<int> ProductCounts { get; set; }
    }
}
