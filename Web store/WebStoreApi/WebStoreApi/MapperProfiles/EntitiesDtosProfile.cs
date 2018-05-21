﻿using AutoMapper;
using System.Linq;
using WebStore.Api.DataTransferObjects;
using WebStore.Api.Models;
using WebStore.Models.Entities;
using WebStore.Models.Enumerations;

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
                .ForMember(dest => dest.AvailableCount, opt => opt.MapFrom(src => src.StorageItems.Count(si => si.State == StorageItemState.Available)));
            CreateMap<Order, OrderDTO>();
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());
            CreateMap<OrderHistory, OrderHistoryDTO>();
            CreateMap<StorageGroupedItemModel, StorageItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<Storage, StorageListItemDTO>();
            CreateMap<StorageGroupedModel, StorageListItemDTO>();
            CreateMap<StorageGroupedModel, StorageDTO>()
                .IncludeBase<StorageGroupedModel, StorageListItemDTO>();


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
