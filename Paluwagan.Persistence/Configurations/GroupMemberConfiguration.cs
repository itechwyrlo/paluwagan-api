using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Configurations
{
    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.ToTable("group_members");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new GroupMemberId(value));

            builder.Property(m => m.GroupId)
                .HasConversion(id => id.Value, value => new GroupId(value))
                .IsRequired();

            builder.Property(m => m.UserId).IsRequired();
            builder.Property(m => m.JoinedAt).IsRequired();
        }
    }
}
