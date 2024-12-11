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
    public class LocalizationMap : IEntityTypeConfiguration<Localization>
    {
        public void Configure(EntityTypeBuilder<Localization> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ResourceKey).IsRequired();
            builder.Property(x => x.Text).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.HasOne<LocalizationCulture>(x => x.Culture).WithMany(y => y.Localizations).HasForeignKey(x => x.CultureId);
            builder.ToTable("Localization");
        }
    }
}
