using AutoMapper;
using WebStore.Api.DataTransferObjects;
using WebStore.Models.Entities;

namespace WebStore.Api.MapperProfiles
{
    public class ModelToDtosProfile : Profile
    {
        public ModelToDtosProfile()
        {
            // ProductCategory
            CreateMap<ProductCategory, ProductCategoryDTO>();
            CreateMap<ProductCategoryDTO, ProductCategory>();

            // ProductSubCategory
            CreateMap<ProductSubCategory, ProductSubCategoryDTO>();
            CreateMap<ProductSubCategoryDTO, ProductSubCategory>();

            // ProductItem
            CreateMap<ProductItem, ProductItemDTO>();
            CreateMap<ProductItemDTO, ProductItem>();

            // CartItem
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CartItemDTO, CartItem>();

            // Order
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();

            // OrderHistory
            CreateMap<OrderHistory, OrderHistoryDTO>();
            CreateMap<OrderHistoryDTO, OrderHistory>();
        }
    }
}
