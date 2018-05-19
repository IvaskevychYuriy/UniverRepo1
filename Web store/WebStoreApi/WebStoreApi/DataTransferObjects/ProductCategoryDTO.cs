using System.Collections.Generic;
using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class ProductCategoryDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductSubCategoryDTO> SubCategories { get; set; }
    }
}
