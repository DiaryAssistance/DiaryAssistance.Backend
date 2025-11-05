using DiaryAssistance.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaryAssistance.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        builder
            .HasOne(u => u.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(u => u.GroupId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}