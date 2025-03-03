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
            x.HasKey("RecordId");
            x.Property(x => x.Title).HasMaxLength(100);
        });

        builder.OwnsMany(x => x.PickupPoints, x =>
        {
            x.ToTable("ExperiencePickupPoints");
            x.WithOwner().HasForeignKey("OwnerId");
            x.HasKey("RecordId");
            x.Property(x => x.Name).HasMaxLength(100);
            x.Property(x => x.Address).HasMaxLength(200);
            x.Property(x => x.Country).HasMaxLength(100);
            x.Property(x => x.State).HasMaxLength(100);
            x.Property(x => x.City).HasMaxLength(100);
            x.Property(x => x.ZipCode).HasMaxLength(20);
            x.Property(x => x.Latitude).HasMaxLength(50);
            x.Property(x => x.Longitude).HasMaxLength(50);
            x.Property(x => x.Time).HasMaxLength(500);
            x.Property(x => x.Details).HasMaxLength(500);
        });

        builder.OwnsMany(x => x.MeetingPoints, x =>
        {
            x.ToTable("ExperienceMeetingPoints");
            x.WithOwner().HasForeignKey("OwnerId");
            x.HasKey("RecordId");
            x.Property(x => x.Name).HasMaxLength(100);
            x.Property(x => x.Address).HasMaxLength(200);
            x.Property(x => x.Country).HasMaxLength(100);
            x.Property(x => x.State).HasMaxLength(100);
            x.Property(x => x.City).HasMaxLength(100);
            x.Property(x => x.ZipCode).HasMaxLength(20);
            x.Property(x => x.Latitude).HasMaxLength(50);
            x.Property(x => x.Longitude).HasMaxLength(50);
            x.Property(x => x.Time).HasMaxLength(500);
            x.Property(x => x.Details).HasMaxLength(500);
        });

        builder.OwnsOne(x => x.TicketRedemptionPoint, x =>
        {
            x.Property(x => x.Name).HasMaxLength(100);
            x.Property(x => x.Address).HasMaxLength(200);
            x.Property(x => x.Country).HasMaxLength(100);
            x.Property(x => x.State).HasMaxLength(100);
            x.Property(x => x.City).HasMaxLength(100);
            x.Property(x => x.ZipCode).HasMaxLength(20);
            x.Property(x => x.Latitude).HasMaxLength(50);
            x.Property(x => x.Longitude).HasMaxLength(50);
            x.Property(x => x.Time).HasMaxLength(500);
            x.Property(x => x.Details).HasMaxLength(500);
        });

        builder.OwnsOne(x => x.EndPoint, x =>
        {
            x.Property(x => x.Name).HasMaxLength(100);
            x.Property(x => x.Address).HasMaxLength(200);
            x.Property(x => x.Country).HasMaxLength(100);
            x.Property(x => x.State).HasMaxLength(100);
            x.Property(x => x.City).HasMaxLength(100);
            x.Property(x => x.ZipCode).HasMaxLength(20);
            x.Property(x => x.Latitude).HasMaxLength(50);
            x.Property(x => x.Longitude).HasMaxLength(50);
            x.Property(x => x.Time).HasMaxLength(500);
            x.Property(x => x.Details).HasMaxLength(500);
        });

        builder.OwnsMany(x => x.Stops, x =>
        {
            x.ToTable("ExperienceStops");
            x.WithOwner().HasForeignKey("OwnerId");
            x.HasKey("RecordId");
            x.Property(x => x.Title).HasMaxLength(100);
            x.Property(x => x.Description).HasMaxLength(1000);
            x.OwnsOne(x => x.Place, x =>
            {
                x.Property(x => x.Name).HasMaxLength(100);
                x.Property(x => x.Address).HasMaxLength(200);
                x.Property(x => x.Country).HasMaxLength(100);
                x.Property(x => x.State).HasMaxLength(100);
                x.Property(x => x.City).HasMaxLength(100);
                x.Property(x => x.ZipCode).HasMaxLength(20);
                x.Property(x => x.Latitude).HasMaxLength(50);
                x.Property(x => x.Longitude).HasMaxLength(50);
            });
            x.OwnsOne(x => x.Duration);
        });

        builder.OwnsMany(x => x.AdditionalInfos, x =>
        {
            x.ToTable("ExperienceAdditionalInfos");
            x.WithOwner().HasForeignKey("OwnerId");
            x.HasKey("RecordId");
            x.Property(x => x.Description).HasMaxLength(500);
        });

        builder.OwnsOne(x => x.Place, x =>
        {
            x.Property(x => x.Name).HasMaxLength(100);
            x.Property(x => x.Address).HasMaxLength(200);
            x.Property(x => x.Country).HasMaxLength(100);
            x.Property(x => x.State).HasMaxLength(100);
            x.Property(x => x.City).HasMaxLength(100);
            x.Property(x => x.ZipCode).HasMaxLength(20);
            x.Property(x => x.Latitude).HasMaxLength(50);
            x.Property(x => x.Longitude).HasMaxLength(50);
        });

        builder.HasIndex(x => new { x.ExperienceId, x.LanguageCode }).IsUnique();
    }
}