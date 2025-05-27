using FabriqPro.Features.Products.Models.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations.MaterialConfigs;

public class MaterialTypeConfigurations : IEntityTypeConfiguration<MaterialType>
{
  public void Configure(EntityTypeBuilder<MaterialType> builder)
  {
    // TODO: qachondir material_types ga o'zgartirib, material_to_departmentni materials qilib qo'yish kerak
    builder.ToTable("materials");
    builder.HasKey(m => m.Id);

    builder.HasIndex(m => m.Title)
      .IsUnique();

    builder.Property(m => m.Id)
      .HasColumnName("id");

    builder.Property(m => m.Title)
      .HasColumnName("title")
      .IsRequired()
      .HasMaxLength(32);

    builder.Property(m => m.Created)
      .HasColumnName("created")
      .IsRequired()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();

    builder.Property(m => m.Updated)
      .HasColumnName("updated")
      .IsRequired()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();
  }
}