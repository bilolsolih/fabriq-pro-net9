using FabriqPro.Features.Products.Models.SparePart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class SparePartDepartmentConfigurations : IEntityTypeConfiguration<SparePartDepartment>
{
  public void Configure(EntityTypeBuilder<SparePartDepartment> builder)
  {
    builder.ToTable("spare_part_department");
    builder.HasKey(sd => new { sd.Department, sd.UserId, sd.SparePartId });

    builder.HasOne(sd => sd.SparePart)
      .WithMany()
      .HasForeignKey(sd => sd.SparePartId);

    builder.HasOne(sd => sd.User)
      .WithMany()
      .HasForeignKey(sd => sd.UserId);

    builder.Property(sd => sd.Department)
      .HasColumnName("department")
      .IsRequired();

    builder.Property(sd => sd.UserId)
      .HasColumnName("user_id")
      .IsRequired();

    builder.Property(sd => sd.SparePartId)
      .HasColumnName("spare_part_id")
      .IsRequired();

    builder.Property(sd => sd.Quantity)
      .HasColumnName("quantity")
      .IsRequired();

    builder.Property(sd => sd.Unit)
      .HasColumnName("unit")
      .IsRequired();
  }
}