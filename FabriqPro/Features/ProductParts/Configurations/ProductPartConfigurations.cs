using FabriqPro.Features.ProductParts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.ProductParts.Configurations;

public class ProductPartConfigurations : IEntityTypeConfiguration<ProductPart>
{
    public void Configure(EntityTypeBuilder<ProductPart> builder)
    {
        builder.ToTable("product_parts");

        builder.HasKey(pp => pp.Id);

        builder.HasIndex(pp => new { pp.ProductTypeId, pp.ProductPartTypeId, pp.ProductModelId })
            .IsUnique();

        builder.Property(pp => pp.Id)
            .HasColumnName("id");

        builder.Property(pp => pp.ProductTypeId)
            .IsRequired()
            .HasColumnName("product_type_id");

        builder.Property(pp => pp.ProductPartTypeId)
            .IsRequired()
            .HasColumnName("product_part_type_id");

        builder.Property(pp => pp.ProductModelId)
            .IsRequired()
            .HasColumnName("product_model_id");

        builder.HasOne(pp => pp.ProductType)
            .WithMany()
            .HasForeignKey(pp => pp.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pp => pp.ProductPartType)
            .WithMany()
            .HasForeignKey(pp => pp.ProductPartTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pp => pp.ProductModel)
            .WithMany()
            .HasForeignKey(pp => pp.ProductModelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}