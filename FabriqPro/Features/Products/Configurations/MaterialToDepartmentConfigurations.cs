using FabriqPro.Features.Products.Models;
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
      .WithMany()
      .HasForeignKey(obj => obj.MaterialId);

    builder.HasOne(obj => obj.Party)
      .WithMany()
      .HasForeignKey(obj => obj.PartyId);

    builder.Property(obj => obj.Department)
      .HasColumnName("department")
      .IsRequired();

    builder.Property(obj => obj.MaterialId)
      .HasColumnName("material_id")
      .IsRequired();

    builder.Property(obj => obj.PartyId)
      .HasColumnName("party_id")
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