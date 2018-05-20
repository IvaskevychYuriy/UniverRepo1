using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class ProductItemDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }

        public int AvailableCount { get; set; }

        public int SubCategoryId { get; set; }
    }
}
