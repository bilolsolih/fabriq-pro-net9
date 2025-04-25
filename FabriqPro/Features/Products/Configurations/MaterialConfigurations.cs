using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class MaterialConfigurations : IEntityTypeConfiguration<Material>
{
  public void Configure(EntityTypeBuilder<Material> builder)
  {
    builder.ToTable("materials");
    builder.HasKey(m => m.Id);
    builder.HasIndex(m => m.Title)
      .IsUnique();

    // builder.HasOne(m => m.ProductType)
    //   .WithMany()
    //   .HasForeignKey(m => m.ProductTypeId)
    //   .OnDelete(DeleteBehavior.Restrict);

    builder.Property(m => m.Id)
      .HasColumnName("id");

    builder.Property(m => m.Title)
      .HasColumnName("title")
      .IsRequired()
      .HasMaxLength(32);
    //
    // builder.Property(m => m.ProductTypeId)
    //   .HasColumnName("product_type_id")
    //   .IsRequired();

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