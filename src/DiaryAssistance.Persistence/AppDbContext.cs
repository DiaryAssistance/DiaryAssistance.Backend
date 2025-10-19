using DiaryAssistance.Core.Entities;
using Microsoft.AspNetCore.Identity; // Исправил Jdentity -> Identity
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiaryAssistance.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // Исправил Base -> base
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Schedule>(entity =>
        {
            entity.Property(s => s.SubjectName) // Убрал "s Schedule =>"
                .HasMaxLength(256);
                  
            entity.Property(s => s.RoomNumber) // Убрал "s Schedule =>"  
                .HasMaxLength(256);

            entity.HasOne(s => s.Teacher) // Убрал "s Schedule =>"
                .WithMany(u => u.Schedules)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.Property(rt => rt.Token)
                .HasMaxLength(256);
        });

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // Исправил AppDoContext -> AppDbContext
    }
}