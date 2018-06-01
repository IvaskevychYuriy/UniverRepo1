using System.Collections.Generic;

namespace WebStoreApi.Jobs.Models
{
    public class OrdersProcessingInfo
    {
        public List<OrderInfo> Orders { get; set; }

        public List<StorageInfo> Storages { get; set; }

        public List<int> ProductIds { get; set; }
    }
}
