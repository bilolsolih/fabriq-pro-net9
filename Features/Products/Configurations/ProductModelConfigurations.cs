using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class ProductModelConfigurations: IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> builder)
    {
        builder.ToTable("product_models");

        builder.HasKey(model => model.Id);

        builder.HasOne(model => model.Color)
            .WithMany(color => color.ProductModels)
            .HasForeignKey(model => model.ColorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(model => model.ProductType)
            .WithMany(type => type.ProductModels)
            .HasForeignKey(model => model.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(model => model.Id)
            .HasColumnName("id");
        
        builder.Property(model => model.ColorId)
            .HasColumnName("color_id")
            .IsRequired();
        
        builder.Property(model => model.ProductTypeId)
            .HasColumnName("product_type_id")
            .IsRequired();
        
        builder.Property(model => model.Title)
            .HasMaxLength(32)
            .HasColumnName("title")
            .IsRequired();
        
        builder.Property(model => model.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("created");

        builder.Property(model => model.Updated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("updated");
    }
    
}