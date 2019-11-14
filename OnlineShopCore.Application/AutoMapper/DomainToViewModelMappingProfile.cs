using AutoMapper;
using OnlineShopCore.Application.ViewModels;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.Entities;

namespace OnlineShopCore.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2);
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Slide, SlideViewModel>();

            CreateMap<Author, AuthorViewModel>();
            CreateMap<Publisher, PublisherViewModel>();

            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);

            CreateMap<Contact, ContactViewModel>().MaxDepth(2);
        }
    }
}
