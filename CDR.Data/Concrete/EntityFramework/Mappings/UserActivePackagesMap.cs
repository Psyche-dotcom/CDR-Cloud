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
    public class UserActivePackagesMap : IEntityTypeConfiguration<UserActivePackages>
    {
        public void Configure(EntityTypeBuilder<UserActivePackages> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.PackageId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.HasOne<User>(x => x.User).WithMany(y => y.ActivePackages).HasForeignKey(x => x.UserId);
            builder.HasOne<Package>(x => x.Package).WithMany(c => c.UserPackages).HasForeignKey(x => x.PackageId);
            builder.ToTable("UserActivePackages");
        }
    }
}
