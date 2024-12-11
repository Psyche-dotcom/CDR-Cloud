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
    public class LocalizationCultureMap : IEntityTypeConfiguration<LocalizationCulture>
    {
        public void Configure(EntityTypeBuilder<LocalizationCulture> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Culture).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.FlagIcon).IsRequired();
            builder.Property(a => a.FlagIcon).HasMaxLength(10);
            builder.ToTable("LocalizationCulture");
        }
    }
}
