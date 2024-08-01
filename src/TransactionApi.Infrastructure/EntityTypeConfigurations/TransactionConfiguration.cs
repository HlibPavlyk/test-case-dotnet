using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Infrastructure.EntityTypeConfigurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.TransactionId);
        
        builder.Property(t => t.TransactionId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        builder.Property(t => t.ClientLocation)
            .IsRequired()
            .HasMaxLength(100);
    }
}