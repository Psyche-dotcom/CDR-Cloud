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
    public class DepositMap : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.PackageId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.PublicId).IsRequired();
            builder.HasOne<User>(x => x.User).WithMany(y => y.Deposits).HasForeignKey(x => x.UserId);
            builder.HasOne<Package>(x => x.Package).WithMany(y => y.Deposits).HasForeignKey(x => x.PackageId);
            builder.ToTable("Deposit");
        }
    }
}
