using System.Collections.Generic;
using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class PageDataDTO : IDataTransferObject
    {
        public List<ProductItemDTO> ProductItems { get; set; }

        public int TotalCount { get; set; }
    }
}
