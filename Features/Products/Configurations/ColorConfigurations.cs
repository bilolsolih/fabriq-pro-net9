using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Products.Configurations;

public class ColorConfigurations : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("colors");

        builder.HasKey(color => color.Id);

        builder.HasIndex(color => color.Title)
            .IsUnique();

        builder.HasIndex(color => color.ColorCode)
            .IsUnique();

        builder.Property(color => color.Id)
            .HasColumnName("id");

        builder.Property(color => color.Title)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("title");

        builder.Property(color => color.ColorCode)
            .HasMaxLength(9)
            .IsRequired()
            .HasColumnName("color_code");

        builder.Property(color => color.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("created");

        builder.Property(color => color.Updated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired()
            .HasColumnName("updated");
    }
}