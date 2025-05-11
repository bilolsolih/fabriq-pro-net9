using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations.ProductConfigs;

public class ProductTypeConfigurations :IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.ToTable("product_types");

        builder.HasKey(type => type.Id);

        builder.HasIndex(type => type.Title);

        builder.Property(type => type.Id)
            .HasColumnName("id");
        
        builder.Property(type => type.Title)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("title");
        
        builder.Property(type => type.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("created");

        builder.Property(type => type.Updated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("updated");
    }
}