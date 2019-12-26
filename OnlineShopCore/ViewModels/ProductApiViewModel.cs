using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.ViewModels
{
    public class ProductApiViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public int PublisherId { get; set; }
        public string Publisher { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string SeoAlias { set; get; }
        public Status Status { get; set; }

        public ProductApiViewModel(Product p)
        {
            Id = p.Id;
            Name = p.Name;
            CategoryId = p.CategoryId;
            Image = p.Image;
            AuthorId = p.AuthorId;
            PublisherId = p.PublisherId;
            Price = p.Price;
            Description = p.Description;
            SeoAlias = p.SeoAlias;
            Status = p.Status;
        }
    }
}

