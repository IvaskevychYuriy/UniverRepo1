using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class CartItemDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public ProductItemDTO Product { get; set; }
        public int Quantity { get; set; }
    }
}
