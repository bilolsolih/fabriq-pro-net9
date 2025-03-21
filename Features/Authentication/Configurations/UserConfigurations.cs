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

        builder.HasOne(user => user.Department)
            .WithMany(department => department.Users)
            .HasForeignKey(user => user.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(user => user.PhoneNumber)
            .IsUnique();

        builder.Property(user => user.Id)
            .HasColumnName("id");
        
        builder.Property(user => user.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired();

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
        builder.Property(user => user.PassportSeries)
            .HasColumnName("passport_series")
            .HasMaxLength(9)
            .IsRequired();
        
        builder.Property(user => user.Birthdate)
            .HasColumnName("birthdate")
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
            // Updated column uchun berilgan qiymatlar ignore bo'ladi : update yoki insert paytida
            .ValueGeneratedOnAdd();
    }
}