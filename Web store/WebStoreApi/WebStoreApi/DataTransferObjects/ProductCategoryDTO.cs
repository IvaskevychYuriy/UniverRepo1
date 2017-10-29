using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class ProductCategoryDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
