using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HolaViaje.Social.Features.Posts.Repository;

internal class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Content).HasMaxLength(4000);


        builder.OwnsOne(x => x.Place,
        place =>
        {
            place.Property(p => p.Country).HasColumnName("Country").HasMaxLength(100);
            place.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
            place.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
            place.OwnsOne(p => p.Location, l =>
            {
                l.Property(p => p.Latitude).HasColumnName("Latitude").HasMaxLength(30);
                l.Property(p => p.Longitude).HasColumnName("Longitude").HasMaxLength(30);
            });
        });

        builder.OwnsMany(x => x.MediaFiles, mf =>
        {
            mf.ToTable("PostFiles");
            mf.WithOwner().HasForeignKey("OwnerId");
            mf.Property<long>("Id");
            mf.HasKey("Id");
            mf.Property(x => x.FileId).HasMaxLength(50);
            mf.Property(x => x.FileName).HasMaxLength(100);
            mf.Property(x => x.ContentType).HasMaxLength(50);
            mf.Property(x => x.ContainerName).HasMaxLength(50);
            mf.Property(x => x.Url).HasMaxLength(2000);
        });

        builder.OwnsMany(x => x.Members, f =>
        {
            f.ToTable("PostMembers");
            f.WithOwner().HasForeignKey("OwnerId");
            f.Property<long>("Id");
            f.HasKey("Id");
        });
    }
}
