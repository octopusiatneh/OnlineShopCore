using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Data.EF
{
    public class DbInitializer
    {
        readonly AppDbContext _context;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
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
                    EmailConfirmed = true
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
                    new Function() {Id = "SYSTEM", Name = "Hệ thống",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fas fa-cogs"  },
                    new Function() {Id = "ROLE", Name = "Nhóm",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "USER", Name = "Người dùng",ParentId = "SYSTEM",SortOrder =2,Status = Status.Active,URL = "/admin/user/index",IconCss = "fas fa-chevron-right"  },

                    new Function() {Id = "PRODUCT",Name = "Sản phẩm",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fas fa-columns"  },
                    new Function() {Id = "PRODUCT_CATEGORY",Name = "Danh mục",ParentId = "PRODUCT",SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "PRODUCT_LIST",Name = "Sản phẩm",ParentId = "PRODUCT",SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "BILL",Name = "Hóa đơn",ParentId = "PRODUCT",SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "AUTHOR",Name = "Tác giả",ParentId = "PRODUCT",SortOrder = 4,Status = Status.Active,URL = "/admin/author/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "PUBLISHER",Name = "Nhà xuất bản",ParentId = "PRODUCT",SortOrder = 5,Status = Status.Active,URL = "/admin/publisher/index",IconCss = "fas fa-chevron-right"  },

                    new Function() {Id = "UTILITY",Name = "Tiện ích",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fas fa-clone"  },
                    new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "NOTIFICATION",Name = "Notification",ParentId = "UTILITY",SortOrder = 2,Status = Status.Active,URL = "/admin/sendnotification/index",IconCss = "fas fa-chevron-right"  },
                    new Function() {Id = "PROMOTION",Name = "Ch. trình khuyến mãi",ParentId = "UTILITY",SortOrder = 3,Status = Status.Active,URL = "/admin/promotion/index",IconCss = "fas fa-chevron-right"  },

                    new Function() {Id = "HOME", Name = "Trang chủ", ParentId = null, SortOrder = 1, Status = Status.Active, URL = "/", IconCss = "fas fa-chevron-right"},
                    new Function() {Id = "DASBOARD", Name = "Dasbooard", ParentId = "HOME", SortOrder = 1, Status = Status.Active, URL = "/admin/home/index", IconCss = "fas fa-home"},

                });
            }

            if (_context.Authors.Count() == 0)
            {
                List<Author> listAuthor = new List<Author>()
                {
                    new Author() {AuthorName="Nguyễn Đình Thi",Status = Status.Active},
                    new Author() {AuthorName="Trần Đăng Khoa", Status = Status.Active},
                    new Author() {AuthorName="Nguyễn Du", Status = Status.Active},
                    new Author() {AuthorName="Jack Fogg", Status = Status.Active}
                };
                _context.Authors.AddRange(listAuthor);
            }

            if (_context.Publishers.Count() == 0)
            {
                List<Publisher> listPublisher = new List<Publisher>()
                {
                    new Publisher() {PublisherName="Kim Đồng", Status = Status.Active},
                    new Publisher() {PublisherName="NXB Trẻ", Status = Status.Active}
                };
                _context.Publishers.AddRange(listPublisher);
            }

            if (_context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name="Kinh dị",SeoAlias="kinh-di",Status=Status.Active,SortOrder=1,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Đối thoại với Thaksin", AuthorId=1, PublisherId=1,DateCreated=DateTime.Now,Image="/uploaded/images/20191026/doi_thoai_voi_thaksin.500x780",SeoAlias = "doi-thoai-voi-thaksin",Price = 169000,Status = Status.Active},

                        }
                    },
                    new ProductCategory() { Name="Lãng mạng",SeoAlias="lang-man",Status=Status.Active ,SortOrder=2,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Sống những ngày không hối tiếc", AuthorId = 2, PublisherId=1,DateCreated=DateTime.Now,Image="/uploaded/images/20191026/song-nhung-ngay-khong-hoi-tiec.450x652.w.b",SeoAlias = "song-nhung-ngay-khong-hoi-tiec",Price = 98000,Status = Status.Active},

                        }},
                    new ProductCategory() { Name="Truyện tranh",SeoAlias="truyen-tranh",Status=Status.Active ,SortOrder=3,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Đồng hành du học cùng con", AuthorId = 3, PublisherId=2,DateCreated=DateTime.Now,Image="/uploaded/images/20191026/dong-hanh-du-hoc-cung-con-tb",SeoAlias = "dong-hanh-du-hoc-cung-con",Price = 78000,Status = Status.Active},

                        }},
                    new ProductCategory() { Name="Kinh tế",SeoAlias="kinh-te",Status=Status.Active,SortOrder=4,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Lưỡng giới", AuthorId = 4, PublisherId=2,DateCreated=DateTime.Now, Image="/uploaded/images/20191026/luong-gioi-bm.450x652.w.b",SeoAlias = "luong-gioi",Price = 329000,Status = Status.Active},

                        }}
                };
                _context.ProductCategories.AddRange(listProductCategory);
            }

            if (_context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                   new Slide("/uploaded/images/20191026/slide1.jpg",Status.Active),
                   new Slide("/uploaded/images/20191026/slide2.jpg",Status.Active)
                };
                _context.Slides.AddRange(listSlide);
            }

            await _context.SaveChangesAsync();
        }
    }
}
