using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Persistence.Data
{
    public class PaluwaganDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public PaluwaganDbContext(
            DbContextOptions<PaluwaganDbContext> options) : base(options)
        {
        }

        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(PaluwaganDbContext).Assembly);

            // Rename Identity tables to match ERD

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("application_users");
                entity.Property(u => u.AccountId)
                    .HasConversion(id => id.Value, value => new AccountId(value))
                    .IsRequired()
                    .HasMaxLength(12);
                entity.HasIndex(u => u.AccountId).IsUnique();
            });
            
            builder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.ToTable("roles");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("user_roles");
            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("user_claims");
            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("user_logins");
            });

            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("user_tokens");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("role_claims");
            });

        }


    }
}