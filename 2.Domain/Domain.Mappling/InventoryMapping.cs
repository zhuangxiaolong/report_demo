using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class InventoryMapping : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("S_Inventory");
            builder.HasKey(t => t.Id);

            //primary key
            builder.Property(t => t.Id).HasColumnName("tracking_code");
            //columns
            builder.Property(t => t.PackCode).HasColumnName("pack_code");
            builder.Property(t => t.SapId).HasColumnName("sap_id");
            builder.Property(t => t.BatchId).HasColumnName("batch_id");
            builder.Property(t => t.ScanDate).HasColumnName("scan_date");
            builder.Property(t => t.StoreId).HasColumnName("store_id");
            builder.Property(t => t.Status).HasColumnName("status");
            builder.Property(t => t.CreateDate).HasColumnName("create_date");
            builder.Property(t => t.UpdateDate).HasColumnName("update_date");
            builder.Property(t => t.SellTimes).HasColumnName("sell_times");
            builder.Property(t => t.ProductDate).HasColumnName("product_date");
            builder.Property(t => t.IsGift).HasColumnName("isgift");
            builder.Property(t => t.ProhibitTwoSales).HasColumnName("prohibitTwoSales");
            builder.Property(t => t.BigArea).HasColumnName("BigArea");
        }
    }
}