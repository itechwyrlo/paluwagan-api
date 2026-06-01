using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new MessageId(value));

            builder.Property(m => m.GroupId)
                .HasConversion(id => id.Value, value => new GroupId(value))
                .IsRequired();

            builder.Property(m => m.SenderId).IsRequired();
            builder.Property(m => m.ReceiverId).IsRequired();

            builder.Property(m => m.Text).HasMaxLength(2000);
            builder.Property(m => m.ImageUrl);

            builder.Property(m => m.SentAt).IsRequired();
            builder.Property(m => m.IsRead).IsRequired();

            builder.Ignore(m => m.DomainEvents);

            builder.HasIndex(m => new { m.GroupId, m.SenderId, m.ReceiverId });
        }
    }
}
