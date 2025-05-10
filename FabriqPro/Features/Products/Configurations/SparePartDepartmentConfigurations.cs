using FabriqPro.Features.Products.Models.SparePart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class SparePartDepartmentConfigurations : IEntityTypeConfiguration<SparePartDepartment>
{
  public void Configure(EntityTypeBuilder<SparePartDepartment> builder)
  {
    builder.ToTable("spare_part_department");
    builder.HasKey(sd => new { sd.Department, sd.ToUserId, sd.SparePartId, sd.Status });

    builder.HasOne(sp => sp.Origin)
      .WithMany(sp => sp.Transfers)
      .HasForeignKey(sp => sp.OriginId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.SparePart)
      .WithMany(m => m.SparePartDepartments)
      .HasForeignKey(obj => obj.SparePartId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.AcceptedUser)
      .WithMany()
      .HasForeignKey(obj => obj.AcceptedUserId)
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

    builder.Property(obj => obj.AcceptedUserId)
      .HasColumnName("accepted_user_id")
      .IsRequired();

    builder.Property(obj => obj.FromUserId)
      .HasColumnName("from_user_id")
      .IsRequired();

    builder.Property(obj => obj.ToUserId)
      .HasColumnName("to_user_id")
      .IsRequired();

    builder.Property(obj => obj.Department)
      .HasColumnName("department")
      .IsRequired();

    builder.Property(obj => obj.SparePartId)
      .HasColumnName("material_id")
      .IsRequired();

    builder.Property(obj => obj.Unit)
      .HasColumnName("unit")
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