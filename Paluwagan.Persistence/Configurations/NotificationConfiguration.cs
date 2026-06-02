using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .HasConversion(id => id.Value, value => new NotificationId(value));

            builder.Property(n => n.UserId).IsRequired();
            builder.Property(n => n.Type).IsRequired();

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Body)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.IsRead).IsRequired();
            builder.Property(n => n.CreatedAt).IsRequired();

            builder.Property(n => n.ReferenceId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Ignore(n => n.DomainEvents);

            builder.HasIndex(n => new { n.UserId, n.CreatedAt });
        }
    }
}
