using FabriqPro.Features.Products.Models.ProductPart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations.ProductPartConfigs;

public class ProductPartConfigurations : IEntityTypeConfiguration<ProductPart>
{
  public void Configure(EntityTypeBuilder<ProductPart> builder)
  {
    builder.ToTable("product_parts");
    builder.HasKey(src => src.Id);

    builder.HasIndex(src => new { src.Department, src.MasterId, src.ProductPartTypeId, src.Status }).IsUnique();

    builder.HasOne(obj => obj.Origin)
      .WithMany(obj => obj.Transfers)
      .HasForeignKey(obj => obj.OriginId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.ProductPartType)
      .WithMany(m => m.ProductParts)
      .HasForeignKey(obj => obj.ProductPartTypeId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.ProductModel)
      .WithMany(m => m.ProductParts)
      .HasForeignKey(obj => obj.ProductModelId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.Master)
      .WithMany()
      .HasForeignKey(obj => obj.MasterId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.FromUser)
      .WithMany()
      .HasForeignKey(obj => obj.FromUserId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.ToUser)
      .WithMany()
      .HasForeignKey(obj => obj.ToUserId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.Property(obj => obj.Id)
      .HasColumnName("id");

    builder.Property(obj => obj.OriginId)
      .HasColumnName("origin_id")
      .IsRequired(false);

    builder.Property(obj => obj.MasterId)
      .HasColumnName("master_id")
      .IsRequired();

    builder.Property(obj => obj.FromUserId)
      .HasColumnName("from_user_id")
      .IsRequired(false);

    builder.Property(obj => obj.ToUserId)
      .HasColumnName("to_user_id")
      .IsRequired(false);

    builder.Property(obj => obj.Department)
      .HasColumnName("department")
      .IsRequired();

    builder.Property(obj => obj.ProductPartTypeId)
      .HasColumnName("product_part_type_id")
      .IsRequired();

    builder.Property(obj => obj.ProductModelId)
      .HasColumnName("product_model_id")
      .IsRequired();

    builder.Property(obj => obj.Quantity)
      .HasColumnName("quantity")
      .IsRequired();

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