using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class ProductSubCategoryDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
