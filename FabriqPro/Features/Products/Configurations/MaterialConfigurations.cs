using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class MaterialConfigurations : IEntityTypeConfiguration<Material>
{
  public void Configure(EntityTypeBuilder<Material> builder)
  {
    builder.ToTable("material_to_department");
    builder.HasKey(m => m.Id);

    builder.HasIndex(obj => new { obj.Department, obj.ToUserId, obj.MaterialId, obj.PartyId, obj.Status })
      .IsUnique();

    builder.HasOne(obj => obj.Origin)
      .WithMany(obj => obj.Transfers)
      .HasForeignKey(obj => obj.OriginId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.AcceptedUser)
      .WithMany(u => u.AcceptedMaterials)
      .HasForeignKey(obj => obj.AcceptedUserId)
      .OnDelete(DeleteBehavior.Restrict);
    
    builder.HasOne(obj => obj.FromUser)
      .WithMany(u => u.SentMaterials)
      .HasForeignKey(obj => obj.FromUserId)
      .OnDelete(DeleteBehavior.Restrict);
    
    builder.HasOne(obj => obj.ToUser)
      .WithMany(u => u.ReceivedMaterials)
      .HasForeignKey(obj => obj.ToUserId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.MaterialType)
      .WithMany(m => m.MaterialDepartments) // for backwards navigation
      .HasForeignKey(obj => obj.MaterialId);

    builder.HasOne(obj => obj.Party)
      .WithMany()
      .HasForeignKey(obj => obj.PartyId);

    builder.HasOne(obj => obj.Color)
      .WithMany()
      .HasForeignKey(obj => obj.ColorId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.Property(obj => obj.Id)
      .HasColumnName("id");
    
    builder.Property(obj => obj.OriginId)
      .HasColumnName("origin_id")
      .IsRequired(false);
    
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

    builder.Property(obj => obj.MaterialId)
      .HasColumnName("material_id")
      .IsRequired();

    builder.Property(obj => obj.PartyId)
      .HasColumnName("party_id")
      .IsRequired();

    builder.Property(obj => obj.ColorId)
      .HasColumnName("color_id")
      .IsRequired();

    builder.Property(obj => obj.Thickness)
      .HasColumnName("thickness")
      .IsRequired();

    builder.Property(obj => obj.Unit)
      .HasColumnName("unit")
      .IsRequired();


    builder.Property(obj => obj.Width)
      .HasColumnName("width")
      .IsRequired();

    builder.Property(obj => obj.HasPatterns)
      .HasColumnName("has_patterns")
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