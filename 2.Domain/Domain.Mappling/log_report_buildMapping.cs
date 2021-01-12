using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class log_report_buildMapping : IEntityTypeConfiguration<log_report_build>
    {
        public void Configure(EntityTypeBuilder<log_report_build> builder)
        {
            builder.ToTable("log_report_build");
            builder.HasKey(t => t.Id);

            //primary key
            builder.Property(t => t.Id);
            //columns
            builder.Property(t => t.report_event_no).HasColumnName("report_event_no");
            builder.Property(t => t.report_build_date).HasColumnName("report_build_date");
            builder.Property(t => t.is_finish).HasColumnName("is_finish");
            builder.Property(t => t.report_build_finish_date).HasColumnName("report_build_finish_date");
        }
    }
}