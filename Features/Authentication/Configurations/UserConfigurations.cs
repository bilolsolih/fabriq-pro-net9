using FabriqPro.Features.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Authentication.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.PhoneNumber)
            .IsUnique();

        builder.Property(user => user.Id)
            .HasColumnName("id");

        builder.Property(user => user.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(user => user.ProfilePhoto)
            .HasColumnName("profile_photo")
            .HasMaxLength(128)
            .IsRequired(false);

        builder.Property(user => user.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(user => user.Password)
            .HasColumnName("password")
            .HasMaxLength(32)
            .IsRequired(false);

        builder.Property(user => user.Address)
            .HasColumnName("address")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(user => user.Role)
            .HasColumnName("role")
            .IsRequired();

        builder.Property(user => user.Salary)
            .HasColumnName("salary")
            .IsRequired(false);

        builder.Property(user => user.WorkingHours)
            .HasColumnName("working_hours")
            .IsRequired(false);

        builder.Property(user => user.WorkingDays)
            .HasColumnName("working_days")
            .HasMaxLength(32)
            .IsRequired(false);

        builder.Property(user => user.Created)
            .HasColumnName("created")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(user => user.Updated)
            .HasColumnName("updated")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();
    }
}