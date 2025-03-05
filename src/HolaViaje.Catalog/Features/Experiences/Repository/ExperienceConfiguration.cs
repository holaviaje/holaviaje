using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HolaViaje.Catalog.Features.Experiences.Repository;

internal class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.ToTable("Experiences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasMany(x => x.Translations)
            .WithOne(x => x.Experience)
            .HasForeignKey(x => x.ExperienceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ComplexProperty(x => x.CancellationPolicy, x =>
        {
            x.Property(x => x.DaysToCancel).HasColumnName("DaysToCancel");
            x.Property(x => x.RefundPercentage).HasColumnName("RefundPercentage");
            x.Property(x => x.Policy).HasColumnName("CancellationPolicyType");
        });

        builder.ComplexProperty(x => x.Duration);

        builder.ComplexProperty(x => x.Control, control =>
        {
            control.Property(c => c.CreatedAt).HasColumnName("CreatedAt");
            control.Property(c => c.LastModifiedAt).HasColumnName("LastModifiedAt");
            control.Property(c => c.DeletedAt).HasColumnName("DeletedAt");
            control.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
        });

        builder.OwnsMany(x => x.Photos, x =>
        {
            x.ToTable("ExperiencePhotos");
            x.WithOwner().HasForeignKey("OwnerId");
            x.HasKey("FileId");
            x.Property(x => x.FileId).HasMaxLength(38);
            x.Property(x => x.ImageUrl).HasMaxLength(2000);
        });
    }
}