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
    public class HelpDetailsMap : IEntityTypeConfiguration<HelpDetails>
    {
        public void Configure(EntityTypeBuilder<HelpDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.LocalizationKey).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.JsText).HasColumnType("NVARCHAR(MAX)");
            builder.Property(x => x.IsActive).IsRequired();
            builder.HasOne<Help>(x => x.Help).WithMany(y => y.HelpDetails).HasForeignKey(x => x.HelpId);
            builder.ToTable("HelpDetails");
        }
    }
}
