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
    public class SupportMessagesMap : IEntityTypeConfiguration<SupportMessages>
    {
        public void Configure(EntityTypeBuilder<SupportMessages> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SupportId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.PublicId).IsRequired();
            builder.Property(x => x.Text).IsRequired();
            builder.Property(x => x.IsAdmin).IsRequired();
            builder.Property(x => x.IsSeenAdmin).IsRequired();
            builder.Property(x => x.IsSeenUser).IsRequired();
            builder.HasOne<Support>(x => x.Support).WithMany(y => y.SupportMessages).HasForeignKey(x => x.SupportId);
            builder.ToTable("SupportMessages");
        }
    }
}
