﻿using Microsoft.AspNetCore.Identity;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopCore.Data.EF
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        public DbInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Description = "Top manager"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    Description = "Staff"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Customer",
                    NormalizedName = "Customer",
                    Description = "Customer"
                });
            }

            if (!_userManager.Users.Any())
            {
                await _userManager.CreateAsync(new AppUser()
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Email = "admin@gmail.com",
                    Balance = 0,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = Status.Active,
                    Avatar = @"/admin-side/assets/images/users/1.jpg",
                    EmailConfirmed=true
                }, "123654$");
                var user = await _userManager.FindByNameAsync("admin");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            if (!_context.Contacts.Any())
            {
                _context.Contacts.Add(new Contact()
                {
                    Id = CommonConstants.DefaultContactId,
                    Address = "155 Sư Vạn Hạnh, Phường 12, Quận 10, TP. Hồ Chí Minh",
                    Email = "ntnq1910@gmail.com",
                    Name = "Coza",
                    Phone = "0904285240",
                    Status = Status.Active,
                    Website = "",
                    Lat = 106.66752,
                    Lng = 10.77592
                });
            }

            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<Function>()
                {
                    new Function() {Id = "SYSTEM", Name = "Hệ thống",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fas fa-cogs"  },
                    new Function() {Id = "ROLE", Name = "Nhóm",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "USER", Name = "Người dùng",ParentId = "SYSTEM",SortOrder =2,Status = Status.Active,URL = "/admin/user/index",IconCss = "fas fa-chevron-right"  },

                    new Function() {Id = "PRODUCT",Name = "Sản phẩm",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fas fa-columns"  },
                    new Function() {Id = "PRODUCT_CATEGORY",Name = "Danh mục",ParentId = "PRODUCT",SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "PRODUCT_LIST",Name = "Sản phẩm",ParentId = "PRODUCT",SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "BILL",Name = "Hóa đơn",ParentId = "PRODUCT",SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fas fa-chevron-right"  },

                    new Function() {Id = "UTILITY",Name = "Tiện ích",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fas fa-clone"  },
                    new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fas fa-chevron-right"  },

                });
            }

            //if (_context.Colors.Count() == 0)
            //{
            //    List<Color> listColor = new List<Color>()
            //    {
            //        new Color() {Name="Đen", Code="#000000" },
            //        new Color() {Name="Trắng", Code="#FFFFFF"},
            //        new Color() {Name="Đỏ", Code="#ff0000" },
            //        new Color() {Name="Xanh", Code="#1000ff" },
            //    };
            //    _context.Colors.AddRange(listColor);
            //}

            //if (_context.Slides.Count() == 0)
            //{
            //    List<Slide> slides = new List<Slide>()
            //    {
            //        new Slide() {Name="Slide 1",Image="/client-side/images/slider/slide-1.jpg",Url="#",DisplayOrder = 0,GroupAlias = "top",Status = Status.Active },
            //        new Slide() {Name="Slide 2",Image="/client-side/images/slider/slide-2.jpg",Url="#",DisplayOrder = 1,GroupAlias = "top",Status = Status.Active },
            //        new Slide() {Name="Slide 3",Image="/client-side/images/slider/slide-3.jpg",Url="#",DisplayOrder = 2,GroupAlias = "top",Status = Status.Active },

            //        new Slide() {Name="Slide 1",Image="/client-side/images/brand1.png",Url="#",DisplayOrder = 1,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 2",Image="/client-side/images/brand2.png",Url="#",DisplayOrder = 2,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 3",Image="/client-side/images/brand3.png",Url="#",DisplayOrder = 3,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 4",Image="/client-side/images/brand4.png",Url="#",DisplayOrder = 4,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 5",Image="/client-side/images/brand5.png",Url="#",DisplayOrder = 5,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 6",Image="/client-side/images/brand6.png",Url="#",DisplayOrder = 6,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 7",Image="/client-side/images/brand7.png",Url="#",DisplayOrder = 7,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 8",Image="/client-side/images/brand8.png",Url="#",DisplayOrder = 8,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 9",Image="/client-side/images/brand9.png",Url="#",DisplayOrder = 9,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 10",Image="/client-side/images/brand10.png",Url="#",DisplayOrder = 10,GroupAlias = "brand",Status = Status.Active },
            //        new Slide() {Name="Slide 11",Image="/client-side/images/brand11.png",Url="#",DisplayOrder = 11,GroupAlias = "brand",Status = Status.Active },
            //    };
            //    _context.Slides.AddRange(slides);
            //}


            //if (_context.Sizes.Count() == 0)
            //{
            //    List<Size> listSize = new List<Size>()
            //    {
            //        new Size() { Name="XXL" },
            //        new Size() { Name="XL"},
            //        new Size() { Name="L" },
            //        new Size() { Name="M" },
            //        new Size() { Name="S" },
            //        new Size() { Name="XS" }
            //    };
            //    _context.Sizes.AddRange(listSize);
            //}

            //if (_context.Categories.Count() == 0)
            //{
            //    List<Category> listProductCategory = new List<Category>()
            //    {
            //        new Category() { Name="Áo nam",SeoAlias="ao-nam",ParentId = null,Status=Status.Active,SortOrder=1,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Sản phẩm 1",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-1",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 2",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-2",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 3",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-3",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 4",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-4",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 5",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-5",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }
            //        },
            //        new Category() { Name="Áo nữ",SeoAlias="ao-nu",ParentId = null,Status=Status.Active ,SortOrder=2,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Sản phẩm 6",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-6",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 7",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-7",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 8",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-8",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 9",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-9",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 10",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-10",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }},
            //        new Category() { Name="Giày nam",SeoAlias="giay-nam",ParentId = null,Status=Status.Active ,SortOrder=3,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Sản phẩm 11",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-11",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 12",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-12",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 13",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-13",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 14",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-14",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 15",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-15",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }},
            //        new Category() { Name="Giày nữ",SeoAlias="giay-nu",ParentId = null,Status=Status.Active,SortOrder=4,
            //            Products = new List<Product>()
            //            {
            //                new Product(){Name = "Sản phẩm 16",DateCreated=DateTime.Now, Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-16",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 17",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-17",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 18",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-18",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 19",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-19",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //                new Product(){Name = "Sản phẩm 20",DateCreated=DateTime.Now,Image="~/admin-side/assets/images/No-image-found.jpg",SeoAlias = "san-pham-20",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
            //            }}
            //    };
            //    _context.Categories.AddRange(listProductCategory);
            //}
       
            await _context.SaveChangesAsync();
        }
    }
}
