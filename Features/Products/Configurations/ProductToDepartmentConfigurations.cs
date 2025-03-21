using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class ProductToDepartmentConfigurations : IEntityTypeConfiguration<ProductToDepartment>
{
    public void Configure(EntityTypeBuilder<ProductToDepartment> builder)
    {
        builder.ToTable("product_to_department");

        builder.HasKey(obj => new { obj.DepartmentId, obj.ProductId });

        builder.HasOne(obj => obj.Product)
            .WithMany()
            .HasForeignKey(obj => obj.ProductId);

        builder.HasOne(obj => obj.Department)
            .WithMany()
            .HasForeignKey(obj => obj.DepartmentId);
        
        builder.Property(obj => obj.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired();
        
        builder.Property(obj => obj.ProductId)
            .HasColumnName("product_id")
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