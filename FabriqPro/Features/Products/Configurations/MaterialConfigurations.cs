using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class MaterialConfigurations : IEntityTypeConfiguration<Material>
{
  public void Configure(EntityTypeBuilder<Material> builder)
  {
    builder.ToTable("materials");
    builder.HasKey(m => m.Id);
    
    builder.HasIndex(m => new { m.ProductTypeId, m.MaterialTypeId })
      .IsUnique();

    builder.HasOne(m => m.ProductType)
      .WithMany()
      .HasForeignKey(m => m.ProductTypeId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(m => m.MaterialType)
      .WithMany()
      .HasForeignKey(m => m.MaterialTypeId);

    builder.Property(m => m.Id)
      .HasColumnName("id");

    builder.Property(m => m.ProductTypeId)
      .HasColumnName("product_type_id")
      .IsRequired();
    
    builder.Property(m => m.MaterialTypeId)
      .HasColumnName("material_type_id")
      .IsRequired();

    builder.Property(m => m.Created)
      .HasColumnName("created")
      .IsRequired()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();

    builder.Property(m => m.Updated)
      .HasColumnName("updated")
      .IsRequired()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();
  }
}