using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.ToTable("products");

    builder.HasKey(product => product.Id);

    builder.HasIndex(product => new { product.ProductTypeId, product.ProductModelId })
      .IsUnique();

    builder.Property(product => product.Id)
      .HasColumnName("id");

    builder.Property(product => product.ProductTypeId)
      .HasColumnName("product_type_id")
      .IsRequired();

    builder.Property(product => product.ProductModelId)
      .HasColumnName("product_model_id")
      .IsRequired();

    builder.Property(product => product.Created)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("created");

    builder.Property(product => product.Updated)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("updated");
  }
}