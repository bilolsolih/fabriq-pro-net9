using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class MaterialToDepartmentConfigurations : IEntityTypeConfiguration<MaterialToDepartment>
{
  public void Configure(EntityTypeBuilder<MaterialToDepartment> builder)
  {
    builder.ToTable("material_to_department");

    builder.HasKey(obj => new { obj.Department, obj.UserId, obj.MaterialId, obj.PartyId });

    builder.HasOne(obj => obj.Material)
      .WithMany(m=>m.MaterialDepartments) // for backwards navigation
      .HasForeignKey(obj => obj.MaterialId);

    builder.HasOne(obj => obj.Party)
      .WithMany()
      .HasForeignKey(obj => obj.PartyId);

    builder.HasOne(obj => obj.Color)
      .WithMany()
      .HasForeignKey(obj => obj.ColorId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.Property(obj => obj.Department)
      .HasColumnName("department")
      .IsRequired();

    builder.Property(obj => obj.MaterialId)
      .HasColumnName("material_id")
      .IsRequired();

    builder.Property(obj => obj.PartyId)
      .HasColumnName("party_id")
      .IsRequired();

    builder.Property(obj => obj.ColorId)
      .HasColumnName("color_id")
      .IsRequired();
    
    builder.Property(obj => obj.Thickness)
      .HasColumnName("thickness")
      .IsRequired();
    
    builder.Property(obj => obj.Unit)
      .HasColumnName("unit")
      .IsRequired();
    
        
    builder.Property(obj => obj.Width)
      .HasColumnName("width")
      .IsRequired();
    
    builder.Property(obj => obj.HasPatterns)
      .HasColumnName("has_patterns")
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