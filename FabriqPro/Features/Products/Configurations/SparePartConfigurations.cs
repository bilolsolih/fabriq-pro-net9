using FabriqPro.Features.Products.Models.SparePart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class SparePartConfigurations : IEntityTypeConfiguration<SparePart>
{
  public void Configure(EntityTypeBuilder<SparePart> builder)
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
      .IsRequired();

    builder.Property(s => s.Updated)
      .HasColumnName("updated")
      .IsRequired();
  }
}