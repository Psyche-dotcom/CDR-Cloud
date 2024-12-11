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
    class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.TransactionId).IsRequired();
            builder.Property(x => x.DepositPublicId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.PublicId).IsRequired();
            builder.ToTable("Transaction");
        }
    }
}
