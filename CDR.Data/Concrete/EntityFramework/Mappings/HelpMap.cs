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
    public class HelpMap : IEntityTypeConfiguration<Help>
    {
        public void Configure(EntityTypeBuilder<Help> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.LocalizationKey).IsRequired();
            builder.Property(x => x.PageType).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.ToTable("Help");
        }
    }
}
