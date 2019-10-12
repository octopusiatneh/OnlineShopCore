using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopCore.Data.EF.Extensions;
using OnlineShopCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopCore.Data.EF.Configurations
{
    public class TagConfiguration : DbEntityConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(50)
               .IsRequired().HasMaxLength(50).IsUnicode(false);
        }
    }
}
