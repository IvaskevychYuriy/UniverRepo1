using AutoMapper;
using System.Linq;
using WebStore.Api.DataTransferObjects;
using WebStore.Models.Entities;

namespace WebStore.Api.MapperProfiles
{
    public class ModelToDtosProfile : Profile
    {
        public ModelToDtosProfile()
        {
            // maps from entities to dtos
            CreateMap<ProductCategory, ProductCategoryDTO>();
            CreateMap<ProductSubCategory, ProductSubCategoryDTO>();
            CreateMap<ProductItem, ProductItemDTO>()
                .ForMember(dest => dest.AvailableCount, opt => opt.MapFrom(src => src.StorageItems.Sum(si => si.Quantity)));
            CreateMap<Order, OrderDTO>();
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<OrderHistory, OrderHistoryDTO>();
            CreateMap<StorageItem, StorageItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductItem.Name));
            CreateMap<Storage, StorageListItemDTO>();
            CreateMap<Storage, StorageDTO>()
                .IncludeBase<Storage, StorageListItemDTO>();


            // maps from dtos to entities
            CreateMap<ProductCategoryDTO, ProductCategory>();
            CreateMap<ProductSubCategoryDTO, ProductSubCategory>();
            CreateMap<ProductItemDTO, ProductItem>();
            CreateMap<CartItemDTO, CartItem>();
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderHistoryDTO, OrderHistory>();
            CreateMap<StorageItemDTO, StorageItem>();
            CreateMap<StorageListItemDTO, Storage>();
        }
    }
}
