using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasConversion(id => id.value, value => new RefreshTokenId(value));

            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Token).IsRequired().HasMaxLength(500);
            builder.Property(r => r.IsUsed).IsRequired();
            builder.Property(r => r.IsRevoked).IsRequired();
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.ExpiryDate).IsRequired();
        }
    }
}
