﻿using AutoMapper;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
                c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<ProductViewModel, Product>()
               .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
               c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
              c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
    
            CreateMap<AppUserViewModel, AppUser>()
             .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
              c.Email, c.PhoneNumber, c.Avatar, c.Status));

            CreateMap<PermissionViewModel, Permission>()
               .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
        }
    }
}
