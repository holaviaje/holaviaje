using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HolaViaje.Catalog.Features.Companies.Repository;

internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.LegalName).HasMaxLength(100);
        builder.Property(x => x.Address1).HasMaxLength(100);
        builder.Property(x => x.Address2).HasMaxLength(100);
        builder.Property(x => x.City).HasMaxLength(50);
        builder.Property(x => x.State).HasMaxLength(50);
        builder.Property(x => x.ZipCode).HasMaxLength(10);
        builder.Property(x => x.Country).HasMaxLength(50);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Website).HasMaxLength(100);
        builder.Property(x => x.RegistrationNumber).HasMaxLength(20);
        builder.Property(x => x.RegisteredIn).HasMaxLength(100);
        builder.Property(x => x.VatId).HasMaxLength(20);

        builder.OwnsOne(x => x.BookInfo);

        builder.OwnsMany(x => x.Managers, x =>
        {
            x.ToTable("CompanyManagers");
            x.WithOwner().HasForeignKey("OwnerId");
            x.Property<int>("Id");
            x.HasKey("Id");
        });
    }
}
