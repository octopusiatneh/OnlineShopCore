﻿using AutoMapper;
using OnlineShopCore.Application.ViewModels;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.AutoMapper
{
   public class DomainToViewModelMappingProfile:Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();
            CreateMap<Slide, SlideViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);

            CreateMap<SlideImage, SlideImageViewModel>().MaxDepth(2);
            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
            CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);

            CreateMap<Feedback, FeedbackViewModel>().MaxDepth(2);
            CreateMap<Contact, ContactViewModel>().MaxDepth(2);
        }
    }
}
