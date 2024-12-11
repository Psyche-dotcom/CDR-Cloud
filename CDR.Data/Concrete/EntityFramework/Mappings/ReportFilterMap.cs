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
    public class ReportFilterMap : IEntityTypeConfiguration<ReportFilter>
    {
        public void Configure(EntityTypeBuilder<ReportFilter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.Json).IsRequired();
            builder.ToTable("ReportFilter");
        }
    }
}
