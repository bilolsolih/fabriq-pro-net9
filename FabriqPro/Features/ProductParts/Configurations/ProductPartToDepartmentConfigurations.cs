using FabriqPro.Features.ProductParts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.ProductParts.Configurations;

public class ProductPartToDepartmentConfigurations : IEntityTypeConfiguration<ProductPartToDepartment>
{
  public void Configure(EntityTypeBuilder<ProductPartToDepartment> builder)
  {
    builder.ToTable("product_part_to_departments");

    builder.HasKey(ppd => ppd.Id);

    builder.HasIndex(ppd => new { ppd.ProductPartId, ppd.Department })
      .IsUnique();

    builder.Property(ppd => ppd.Id)
      .HasColumnName("id");

    builder.Property(ppd => ppd.ProductPartId)
      .IsRequired()
      .HasColumnName("product_part_id");

    builder.Property(ppd => ppd.Department)
      .IsRequired()
      .HasColumnName("department");

    builder.Property(ppd => ppd.Quantity)
      .IsRequired()
      .HasColumnName("quantity");

    builder.Property(ppd => ppd.Created)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("created");

    builder.Property(ppd => ppd.Updated)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("updated");

    builder.HasOne(ppd => ppd.ProductPart)
      .WithMany()
      .HasForeignKey(ppd => ppd.ProductPartId)
      .OnDelete(DeleteBehavior.Restrict);

    // builder.HasOne(ppd => ppd.Department)
    //     .WithMany()
    //     .HasForeignKey(ppd => ppd.DepartmentId)
    //     .OnDelete(DeleteBehavior.Restrict);
  }
}