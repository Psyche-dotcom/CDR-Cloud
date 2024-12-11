using CDR.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Data.Concrete.EntityFramework.Mappings
{
    public class ContentMap : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ContentType).IsRequired();
            builder.Property(x => x.LocalizationCultureId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.Text).HasColumnType("NVARCHAR(MAX)");
            builder.HasOne<LocalizationCulture>(x => x.LocalizationCulture).WithMany(y => y.Contents).HasForeignKey(x => x.LocalizationCultureId);
            builder.ToTable("Content");
        }
    }
}
