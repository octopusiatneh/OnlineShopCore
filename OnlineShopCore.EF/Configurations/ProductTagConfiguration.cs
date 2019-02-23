using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopCore.Data.EF.Extensions;
using OnlineShopCore.Data.Entities;

namespace OnlineShopCore.Data.EF.Configurations
{
    public class ProductTagConfiguration : DbEntityConfiguration<ProductTag>
    {
        public override void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.Property(c => c.TagId).HasMaxLength(255).IsRequired()
            .HasColumnType("varchar(255)");
            // etc.
        }
    }
}
