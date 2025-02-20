using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HolaViaje.Social.Features.Profiles.Repository;

internal class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.AboutMe).HasMaxLength(1000);

        builder.OwnsOne(x => x.Availability, availability =>
        {
            availability.Property(a => a.IsAvailable).HasColumnName("IsAvailable");
            availability.Property(x => x.AvailableFor).HasMaxLength(250);
        });

        builder.OwnsOne(x => x.Control, control =>
        {
            control.Property(c => c.CreatedAt).HasColumnName("CreatedAt");
            control.Property(c => c.LastModifiedAt).HasColumnName("LastModifiedAt");
            control.Property(c => c.DeletedAt).HasColumnName("DeletedAt");
            control.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
        });

        builder.OwnsMany(x => x.SpokenLanguages, sl =>
        {
            sl.ToTable("ProfileSpokenLanguages");
            sl.WithOwner().HasForeignKey("OwnerId");
            sl.Property<long>("Id");
            sl.HasKey("Id");
            sl.Property(x => x.Code).HasMaxLength(10);
            sl.Property(x => x.Language).HasMaxLength(50);
        });

        builder.OwnsOne(x => x.Picture);
        builder.OwnsOne(x => x.Place, place =>
        {
            place.Property(p => p.Country).HasColumnName("Country").HasMaxLength(100);
            place.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
            place.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
            place.OwnsOne(p => p.Location, l =>
            {
                l.Property(p => p.Latitude).HasColumnName("Latitude").HasMaxLength(50);
                l.Property(p => p.Longitude).HasColumnName("Longitude").HasMaxLength(50);
            });
        });
    }
}
