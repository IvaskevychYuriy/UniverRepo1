using System.Collections.Generic;
using WebStore.Models.Models;

namespace WebStoreApi.Jobs.Models
{
    public class OrderInfo
    {
        public int OrderId { get; set; }

        public AddressCoordinates Coordinates { get; set; }

        public List<int> ProductCounts { get; set; }
    }
}
