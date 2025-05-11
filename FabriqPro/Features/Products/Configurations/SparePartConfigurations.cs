using FabriqPro.Features.Products.Models.SparePart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class SparePartConfigurations : IEntityTypeConfiguration<SparePartType>
{
  public void Configure(EntityTypeBuilder<SparePartType> builder)
  {
    builder.ToTable("spare_parts");
    builder.HasKey(s => s.Id);
    
    builder.HasIndex(s => s.Title)
      .IsUnique();

    builder.Property(s => s.Id)
      .HasColumnName("id");

    builder.Property(s => s.Title)
      .HasColumnName("title")
      .HasMaxLength(64)
      .IsRequired();

    builder.Property(s => s.Created)
      .HasColumnName("created")
      .ValueGeneratedOnAdd()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .IsRequired();

    builder.Property(s => s.Updated)
      .HasColumnName("updated")
      .ValueGeneratedOnAdd()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .IsRequired();
  }
}