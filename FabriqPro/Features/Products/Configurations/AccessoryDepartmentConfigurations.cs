using FabriqPro.Features.Products.Models.Accessory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class AccessoryDepartmentConfigurations : IEntityTypeConfiguration<Accessory>
{
  public void Configure(EntityTypeBuilder<Accessory> builder)
  {
    builder.ToTable("accessory_department");
    builder.HasKey(src => src.Id);

    builder.HasIndex(sd => new { sd.Department, sd.ToUserId, AccessoryId = sd.AccessoryTypeId, sd.Status }).IsUnique();

    builder.HasOne(obj => obj.Origin)
      .WithMany(obj => obj.Transfers)
      .HasForeignKey(obj => obj.OriginId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(obj => obj.AccessoryType)
      .WithMany(m => m.AccessoryDepartments)
      .HasForeignKey(obj => obj.AccessoryTypeId)
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

    builder.Property(obj => obj.AccessoryTypeId)
      .HasColumnName("accessory_id")
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