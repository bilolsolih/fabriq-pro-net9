using FabriqPro.Features.Products.Models.ProductPart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations.ProductPartConfigs;

public class ProductPartTypeConfigurations : IEntityTypeConfiguration<ProductPartType>
{
  public void Configure(EntityTypeBuilder<ProductPartType> builder)
  {
    builder.ToTable("product_part_types");

    builder.HasKey(obj => obj.Id);

    builder.HasOne(obj => obj.ProductType)
      .WithMany()
      .HasForeignKey(obj => obj.ProductTypeId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(obj => obj.Title)
      .IsUnique();

    builder.Property(obj => obj.Id)
      .HasColumnName("id");
    
    builder.Property(obj => obj.ProductTypeId)
      .HasColumnName("product_type_id");

    builder.Property(obj => obj.Title)
      .HasMaxLength(64)
      .IsRequired()
      .HasColumnName("title");

    builder.Property(obj => obj.Created)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("created");

    builder.Property(obj => obj.Updated)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("updated");
  }
}