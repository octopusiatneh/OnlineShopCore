using OnlineShopCore.Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Application.ViewModels.Product
{
    [Serializable]
    public class ProductViewModel
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public int TotalPage { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public int PublisherId { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [DefaultValue(0)]
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int ViewCount { get; set; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }

        public virtual AuthorViewModel Author { set; get; }

        public virtual PublisherViewModel Publisher { get; set; }

        [StringLength(255)]
        public string SeoAlias { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public Status Status { set; get; }
    }
}
