using FabriqPro.Features.Products.Models.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class MiscellaneousTypeConfigurations : IEntityTypeConfiguration<MiscellaneousType>
{
  public void Configure(EntityTypeBuilder<MiscellaneousType> builder)
  {
    builder.ToTable("miscellaneous");
    builder.HasKey(a => a.Id);

    builder.HasIndex(a => a.Title)
      .IsUnique();

    builder.Property(a => a.Id)
      .HasColumnName("id");

    builder.Property(a => a.Title)
      .HasColumnName("title")
      .HasMaxLength(64)
      .IsRequired();

    builder.Property(a => a.Created)
      .HasColumnName("created")
      .ValueGeneratedOnAdd()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .IsRequired();

    builder.Property(a => a.Updated)
      .HasColumnName("updated")
      .ValueGeneratedOnAdd()
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .IsRequired();
  }
}