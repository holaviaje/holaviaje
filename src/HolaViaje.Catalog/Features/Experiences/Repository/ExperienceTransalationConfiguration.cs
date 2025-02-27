using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HolaViaje.Catalog.Features.Experiences.Repository;

internal class ExperienceTransalationConfiguration : IEntityTypeConfiguration<ExperienceTranslation>
{
    public void Configure(EntityTypeBuilder<ExperienceTranslation> builder)
    {
        builder.ToTable("ExperienceTranslations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.LanguageCode).HasMaxLength(10);
        builder.Property(x => x.Title).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.CancellationPolicyName).HasMaxLength(100);
        builder.Property(x => x.CancellationPolicyDetails).HasMaxLength(1000);
        builder.Property(x => x.ImportantInformation).HasMaxLength(1000);
        builder.Property(x => x.WhatToExpect).HasMaxLength(2000);
        builder.Property(x => x.LiveGuide).HasMaxLength(100);

        builder.OwnsMany(x => x.Services, x =>
        {
            x.ToTable("ExperienceServices");
            x.WithOwner().HasForeignKey("OwnerId");
            x.Property<int>("Id");
            x.HasKey("Id");
            x.Property(x => x.Title).HasMaxLength(100);
        });

        builder.OwnsOne(x => x.Pickup, x =>
        {
            x.Property(x => x.Address1).HasMaxLength(100);
            x.Property(x => x.Address2).HasMaxLength(100);
            x.OwnsOne(x => x.Place, p =>
            {
                p.Property(p => p.Country).HasMaxLength(100);
                p.Property(p => p.State).HasMaxLength(100);
                p.Property(p => p.City).HasMaxLength(100);
                p.OwnsOne(p => p.Location, l =>
                {
                    l.Property(p => p.Latitude).HasMaxLength(30);
                    l.Property(p => p.Longitude).HasMaxLength(30);
                });
            });
        });

        builder.OwnsOne(x => x.Place, x =>
        {
            x.Property(x => x.Country).HasMaxLength(100);
            x.Property(x => x.State).HasMaxLength(100);
            x.Property(x => x.City).HasMaxLength(100);
            x.OwnsOne(x => x.Location, l =>
            {
                l.Property(p => p.Latitude).HasMaxLength(30);
                l.Property(p => p.Longitude).HasMaxLength(30);
            });
        });

        builder.OwnsMany(x => x.Stops, x =>
        {
            x.ToTable("ExperienceStops");
            x.WithOwner().HasForeignKey("OwnerId");
            x.Property<int>("Id");
            x.HasKey("Id");
            x.Property(x => x.Title).HasMaxLength(100);
            x.Property(x => x.Description).HasMaxLength(500);
            x.Property(x => x.AdditionalInfo).HasMaxLength(100);
            x.OwnsOne(x => x.Place, p =>
            {
                p.Property(p => p.Country).HasMaxLength(100);
                p.Property(p => p.State).HasMaxLength(100);
                p.Property(p => p.City).HasMaxLength(100);
                p.OwnsOne(p => p.Location, l =>
                {
                    l.Property(p => p.Latitude).HasMaxLength(30);
                    l.Property(p => p.Longitude).HasMaxLength(30);
                });
            });
        });

        builder.OwnsMany(x => x.AdditionalInfos, x =>
        {
            x.ToTable("ExperienceAdditionalInfos");
            x.WithOwner().HasForeignKey("OwnerId");
            x.Property<int>("Id");
            x.HasKey("Id");
            x.Property(x => x.Description).HasMaxLength(500);
        });

        builder.HasIndex(x => new { x.ExperienceId, x.LanguageCode }).IsUnique();
    }
}