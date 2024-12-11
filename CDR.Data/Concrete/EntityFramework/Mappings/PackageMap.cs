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
    public class PackageMap : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.MonthPrice).IsRequired();
            builder.Property(x => x.YearPrice).IsRequired();
            builder.Property(x => x.Currency).IsRequired();
            builder.Property(x => x.Month).IsRequired();
            builder.Property(x => x.IsTrial).IsRequired();
            builder.Property(x => x.IsShown).IsRequired();
            builder.Property(x => x.SimultaneousCalls).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.PublicId).IsRequired();
            builder.ToTable("Package");
        }
    }
}
