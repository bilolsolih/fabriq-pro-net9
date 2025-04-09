using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(department => department.Id);

        builder.HasMany(department => department.Products)
            .WithMany(product => product.Departments)
            .UsingEntity<ProductToDepartment>();

        builder.Property(department => department.Id)
            .HasColumnName("id");
        
        builder.Property(department => department.Title)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("title");
            
        builder.Property(department => department.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("created");

        builder.Property(department => department.Updated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("updated");
    }
    
}