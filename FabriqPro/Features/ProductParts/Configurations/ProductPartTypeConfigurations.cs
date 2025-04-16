using FabriqPro.Features.ProductParts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.ProductParts.Configurations;

public class ProductPartTypeConfigurations : IEntityTypeConfiguration<ProductPartType>
{
    public void Configure(EntityTypeBuilder<ProductPartType> builder)
    {
        builder.ToTable("product_part_types");

        builder.HasKey(ppt => ppt.Id);

        builder.HasIndex(ppt => new { ppt.Title, ppt.ProductTypeId })
            .IsUnique();

        builder.Property(ppt => ppt.Id)
            .HasColumnName("id");

        builder.Property(ppt => ppt.Title)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("title");

        builder.Property(ppt => ppt.ProductTypeId)
            .IsRequired()
            .HasColumnName("product_type_id");

        builder.Property(ppt => ppt.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("created");

        builder.Property(ppt => ppt.Updated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("updated");

        builder.HasOne(ppt => ppt.ProductType)
            .WithMany()
            .HasForeignKey(ppt => ppt.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}