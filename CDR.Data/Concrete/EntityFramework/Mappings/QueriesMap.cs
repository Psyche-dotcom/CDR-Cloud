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
    public class QueriesMap : IEntityTypeConfiguration<Queries>
    {
        public void Configure(EntityTypeBuilder<Queries> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.JsonData).IsRequired();
            builder.Property(x => x.JsonData).HasColumnType("NVARCHAR(MAX)");
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.IpAddress).IsRequired();
            builder.Property(x => x.Port).IsRequired();
            builder.Property(x => x.DbName).IsRequired();
            builder.Property(x => x.DbUsername).IsRequired();
            builder.Property(x => x.DbPassword).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.ToTable("Queries");
        }
    }
}
