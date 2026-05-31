using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("groups");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasConversion(id => id.Value, value => new GroupId(value));

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.ContributionAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(g => g.Schedule).IsRequired();
            builder.Property(g => g.NumberOfSlots).IsRequired();
            builder.Property(g => g.StartDate).IsRequired();
            builder.Property(g => g.OrganizerId).IsRequired();
            builder.Property(g => g.CreatedAt).IsRequired();

            builder.Ignore(g => g.DomainEvents);

            builder.HasMany(g => g.Members)
                .WithOne()
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(g => g.Members)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
