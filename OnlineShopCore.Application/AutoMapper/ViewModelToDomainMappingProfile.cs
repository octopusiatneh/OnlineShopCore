using AutoMapper;
using OnlineShopCore.Application.ViewModels;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.Entities;
using System;

namespace OnlineShopCore.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AuthorViewModel, Author>()
                .ConstructUsing(c => new Author(c.Id, c.AuthorName, c.ParentId, c.SortOrder, c.Status));

            CreateMap<PublisherViewModel, Publisher>()
                .ConstructUsing(c => new Publisher(c.Id, c.NamePublisher, c.ParentId, c.SortOrder, c.Status));

            CreateMap<PromotionViewModel, Promotion>()
                .ConstructUsing(c => new Promotion(c.Id, c.PromotionName, c.DateEnd, c.Status));

            CreateMap<PromotionDetailViewModel, PromotionDetail>()
             .ConstructUsing(c => new PromotionDetail(c.Id, c.PromotionId, c.ProductId, c.PromotionPercent));

            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag, c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<ProductViewModel, Product>()
               .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.AuthorId, c.PublisherId, c.Image, c.Price, c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Status, c.SeoAlias, c.ViewCount));

            CreateMap<AppUserViewModel, AppUser>()
             .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.Address, c.UserName, c.Email, c.PhoneNumber, c.Avatar, c.Province, c.DistrictID, c.WardCode, c.Status));

            CreateMap<PermissionViewModel, Permission>()
               .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));

            CreateMap<BillViewModel, Bill>()
                    .ConstructUsing(c => new Bill(c.Id, c.CustomerName, c.CustomerAddress, c.ServiceID, c.Province, c.DistrictID, c.WardCode, c.CODAmount, c.CustomerMobile, c.CustomerMessage, c.BillStatus, c.PaymentMethod, c.Status, c.CustomerId));

            CreateMap<BillDetailViewModel, BillDetail>()
             .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId, c.Quantity, c.Price));

            CreateMap<ContactViewModel, Contact>()
               .ConstructUsing(c => new Contact(c.Id, c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));

            CreateMap<SlideViewModel, Slide>()
              .ConstructUsing(c => new Slide(c.Id, c.Image, c.Status));

            CreateMap<AnnouncementViewModel, Announcement>()
                .ConstructUsing(c => new Announcement(c.Title, c.Content, c.Status));

            CreateMap<AnnouncementBillViewModel, AnnouncementBill>()
               .ConstructUsing(c => new AnnouncementBill(c.AnnouncementId, c.HasRead));
        }
    }
}
