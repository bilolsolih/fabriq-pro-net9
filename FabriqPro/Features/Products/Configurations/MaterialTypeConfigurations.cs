using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class MaterialTypeConfigurations : IEntityTypeConfiguration<MaterialType>
{
  public void Configure(EntityTypeBuilder<MaterialType> builder)
  {
    builder.ToTable("material_types");

    builder.HasKey(mt => mt.Id);
    builder.HasIndex(mt => mt.Title)
      .IsUnique();

    builder.HasMany(mt => mt.ProductTypes)
      .WithMany(pt => pt.MaterialTypes);

    builder.Property(mt => mt.Id)
      .HasColumnName("id")
      .IsRequired();

    builder.Property(mt => mt.Title)
      .HasColumnName("title")
      .IsRequired()
      .HasMaxLength(32);

    builder.Property(mt => mt.Created)
      .HasColumnName("created")
      .IsRequired()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();

    builder.Property(mt => mt.Updated)
      .HasColumnName("updated")
      .IsRequired()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();
  }
}