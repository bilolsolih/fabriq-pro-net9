using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class PartyConfigurations : IEntityTypeConfiguration<Party>
{
  public void Configure(EntityTypeBuilder<Party> builder)
  {
    builder.ToTable("parties");

    builder.HasKey(p => p.Id);

    builder.HasIndex(p => p.Title)
      .IsUnique();
    
    builder.Property(p => p.Id)
      .HasColumnName("id");

    builder.Property(p => p.Title)
      .HasMaxLength(32)
      .IsRequired()
      .HasColumnName("title");

    builder.Property(p => p.Created)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("created");

    builder.Property(p => p.Updated)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd()
      .IsRequired()
      .HasColumnName("updated");
  }
}