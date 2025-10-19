using DiaryAssistance.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaryAssistance.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId);

        builder
            .Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(128);

        builder
            .Property(x => x.Expires)
            .HasColumnName("Expires")
            .IsRequired();

        builder
            .HasIndex(x => x.Token)
            .IsUnique();
    }
}