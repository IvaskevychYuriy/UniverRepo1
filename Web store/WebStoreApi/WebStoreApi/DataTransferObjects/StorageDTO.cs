using System.Collections.Generic;

namespace WebStore.Api.DataTransferObjects
{
    public class StorageListItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StorageDTO : StorageListItemDTO
    {
        public List<StorageItemDTO> Items { get; set; }
    }
}
