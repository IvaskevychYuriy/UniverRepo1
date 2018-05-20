using System.Collections.Generic;

namespace WebStore.Models.Entities
{
    public class Storage : EntityBase<int>
    {
        public Storage()
        {
            Items = new HashSet<StorageItem>();
        }

        public string Name { get; set; }

        public ICollection<StorageItem> Items { get; set; }
    }
}
