using FabriqPro.Features.Products.Models.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations.MaterialConfigs;

public class CuttingConfigurations : IEntityTypeConfiguration<Cutting>
{
  public void Configure(EntityTypeBuilder<Cutting> builder)
  {
    builder.ToTable("cuttings");
    builder.HasKey(obj => obj.Id);

    builder.HasOne(obj => obj.Master)
      .WithMany()
      .HasForeignKey(obj => obj.MasterId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(obj => obj.MaterialsUsed)
      .WithMany();

    builder.HasMany(obj => obj.ProducedParts)
      .WithMany();

    builder.Property(obj => obj.Id)
      .HasColumnName("id");

    builder.Property(obj => obj.MasterId)
      .IsRequired()
      .HasColumnName("master_id");
    
    builder.Property(obj => obj.Waste)
      .IsRequired()
      .HasColumnName("waste");
    
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