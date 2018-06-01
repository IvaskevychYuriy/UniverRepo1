using System.Collections.Generic;
using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class StorageListItemDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AddressCoordinatesDTO Coordinates { get; set; }
    }

    public class StorageDTO : StorageListItemDTO
    {
        public List<StorageItemDTO> Items { get; set; }
        public List<DroneDTO> Drones { get; set; }
    }
}
