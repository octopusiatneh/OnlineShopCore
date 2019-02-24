﻿using AutoMapper;
using OnlineShopCore.Application.ViewModels.Product;
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

        }
    }
}