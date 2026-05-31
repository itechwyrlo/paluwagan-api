using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => new PaymentId(value));

            builder.Property(p => p.GroupId)
                .HasConversion(id => id.Value, value => new GroupId(value))
                .IsRequired();

            builder.Property(p => p.MemberId).IsRequired();
            builder.Property(p => p.Round).IsRequired();
            builder.Property(p => p.IsPaid).IsRequired();
            builder.Property(p => p.PaidAt);
        }
    }
}
