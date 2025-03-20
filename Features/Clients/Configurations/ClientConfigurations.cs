using FabriqPro.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabriqPro.Features.Clients.Configurations;

public class ClientConfigurations : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        
        builder.HasKey(client => client.Id);

        builder.Property(client => client.FirstName)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("first_name");
        
        builder.Property(client => client.LastName)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("last_name");
        
        builder.Property(client => client.PhoneNumber)
            .HasMaxLength(32)
            .IsRequired()
            .HasColumnName("phone_number");
        
        builder.Property(client => client.Address)
            .HasMaxLength(128)
            .IsRequired()
            .HasColumnName("address");
        
        builder.Property(user => user.Created)
            .HasColumnName("created")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(user => user.Updated)
            .HasColumnName("updated")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}