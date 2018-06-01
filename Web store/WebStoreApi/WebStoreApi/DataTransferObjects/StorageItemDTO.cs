using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class StorageItemDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int StorageId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
