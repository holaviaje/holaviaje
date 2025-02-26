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

        builder.OwnsOne(x => x.CancellationPolicy, x =>
        {
            x.Property(x => x.DaysToCancel).HasColumnName("DaysToCancel");
            x.Property(x => x.RefundPercentage).HasColumnName("RefundPercentage");
            x.Property(x => x.Policy).HasColumnName("CancellationPolicyType");
        });

        builder.OwnsOne(x => x.TimeRange, x =>
        {
            x.Property(x => x.StartTime).HasColumnName("StartTime");
            x.Property(x => x.EndTime).HasColumnName("EndTime");
            x.Property(x => x.Duration).HasColumnName("Duration");
        });

        builder.OwnsOne(x => x.BookInfirmation, x =>
        {
            x.Property(x => x.Phone).HasColumnName("Phone");
            x.Property(x => x.WhatsApp).HasColumnName("WhatsApp");
            x.Property(x => x.Email).HasColumnName("Email");
        });

        builder.OwnsMany(x => x.Photos, x =>
        {
            x.ToTable("ExperiencePhotos");
            x.WithOwner().HasForeignKey("OwnerId");
            x.Property<int>("Id");
            x.HasKey("Id");
            x.Property(x => x.FileId).HasMaxLength(50);
            x.Property(x => x.ImageUrl).HasMaxLength(2000);
        });

        builder.OwnsOne(x => x.Control, control =>
        {
            control.Property(c => c.CreatedAt).HasColumnName("CreatedAt");
            control.Property(c => c.LastModifiedAt).HasColumnName("LastModifiedAt");
            control.Property(c => c.DeletedAt).HasColumnName("DeletedAt");
            control.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
        });
    }
}