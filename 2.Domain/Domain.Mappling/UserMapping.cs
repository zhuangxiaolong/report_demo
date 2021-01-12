using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("M_User");
            builder.HasKey(t => t.Id);

            //primary key
            builder.Property(t => t.Id);
            //columns
            builder.Property(t => t.Name).HasColumnName("Name").HasMaxLength(100);
            builder.Property(t => t.Pw).HasColumnName("Pw");
        }
    }
}