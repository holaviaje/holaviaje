﻿// <auto-generated />
using System;
using HolaViaje.Social.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HolaViaje.Social.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250224004643_AddPostEntity")]
    partial class AddPostEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HolaViaje.Social.Features.Posts.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)");

                    b.Property<bool>("EditMode")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsHtmlContent")
                        .HasColumnType("boolean");

                    b.Property<int>("PageId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Visibility")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("HolaViaje.Social.Features.Profiles.UserProfile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AboutMe")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TravelCompanion")
                        .HasColumnType("integer");

                    b.Property<int>("Visibility")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles", (string)null);
                });

            modelBuilder.Entity("HolaViaje.Social.Features.Posts.Post", b =>
                {
                    b.OwnsMany("HolaViaje.Social.Features.Posts.MediaFile", "MediaFiles", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<long>("Id"));

                            b1.Property<string>("ContainerName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("FileId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<long>("FileSize")
                                .HasColumnType("bigint");

                            b1.Property<long>("OwnerId")
                                .HasColumnType("bigint");

                            b1.Property<bool>("Uploaded")
                                .HasColumnType("boolean");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)");

                            b1.HasKey("Id");

                            b1.HasIndex("OwnerId");

                            b1.ToTable("PostFiles", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OwnerId");
                        });

                    b.OwnsMany("HolaViaje.Social.Features.Posts.PostMember", "Members", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<long>("Id"));

                            b1.Property<long>("OwnerId")
                                .HasColumnType("bigint");

                            b1.Property<long>("UserId")
                                .HasColumnType("bigint");

                            b1.HasKey("Id");

                            b1.HasIndex("OwnerId");

                            b1.ToTable("PostMembers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OwnerId");
                        });

                    b.OwnsOne("HolaViaje.Social.Shared.EntityControl", "Control", b1 =>
                        {
                            b1.Property<long>("PostId")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("CreatedAt");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("DeletedAt");

                            b1.Property<bool>("IsDeleted")
                                .HasColumnType("boolean")
                                .HasColumnName("IsDeleted");

                            b1.Property<DateTime>("LastModifiedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("LastModifiedAt");

                            b1.HasKey("PostId");

                            b1.ToTable("Posts");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsOne("HolaViaje.Social.Shared.PlaceInfo", "Place", b1 =>
                        {
                            b1.Property<long>("PostId")
                                .HasColumnType("bigint");

                            b1.Property<string>("City")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("Country");

                            b1.Property<string>("State")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("State");

                            b1.HasKey("PostId");

                            b1.ToTable("Posts");

                            b1.WithOwner()
                                .HasForeignKey("PostId");

                            b1.OwnsOne("HolaViaje.Social.Shared.LocationInfo", "Location", b2 =>
                                {
                                    b2.Property<long>("PlaceInfoPostId")
                                        .HasColumnType("bigint");

                                    b2.Property<string>("Latitude")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)")
                                        .HasColumnName("Latitude");

                                    b2.Property<string>("Longitude")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)")
                                        .HasColumnName("Longitude");

                                    b2.HasKey("PlaceInfoPostId");

                                    b2.ToTable("Posts");

                                    b2.WithOwner()
                                        .HasForeignKey("PlaceInfoPostId");
                                });

                            b1.Navigation("Location");
                        });

                    b.Navigation("Control")
                        .IsRequired();

                    b.Navigation("MediaFiles");

                    b.Navigation("Members");

                    b.Navigation("Place")
                        .IsRequired();
                });

            modelBuilder.Entity("HolaViaje.Social.Features.Profiles.UserProfile", b =>
                {
                    b.OwnsMany("HolaViaje.Social.Features.Profiles.SpokenLanguage", "SpokenLanguages", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<long>("Id"));

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.Property<string>("Language")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<long>("OwnerId")
                                .HasColumnType("bigint");

                            b1.HasKey("Id");

                            b1.HasIndex("OwnerId");

                            b1.ToTable("ProfileSpokenLanguages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OwnerId");
                        });

                    b.OwnsOne("HolaViaje.Social.Features.Profiles.Availability", "Availability", b1 =>
                        {
                            b1.Property<long>("UserProfileId")
                                .HasColumnType("bigint");

                            b1.Property<string>("AvailableFor")
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)");

                            b1.Property<bool>("IsAvailable")
                                .HasColumnType("boolean")
                                .HasColumnName("IsAvailable");

                            b1.HasKey("UserProfileId");

                            b1.ToTable("UserProfiles");

                            b1.WithOwner()
                                .HasForeignKey("UserProfileId");
                        });

                    b.OwnsOne("HolaViaje.Social.Features.Profiles.ProfilePicture", "Picture", b1 =>
                        {
                            b1.Property<long>("UserProfileId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Filename")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("ImageUrl")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<DateTime>("LastModifiedAt")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("UserProfileId");

                            b1.ToTable("UserProfiles");

                            b1.WithOwner()
                                .HasForeignKey("UserProfileId");
                        });

                    b.OwnsOne("HolaViaje.Social.Shared.EntityControl", "Control", b1 =>
                        {
                            b1.Property<long>("UserProfileId")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("CreatedAt");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("DeletedAt");

                            b1.Property<bool>("IsDeleted")
                                .HasColumnType("boolean")
                                .HasColumnName("IsDeleted");

                            b1.Property<DateTime>("LastModifiedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("LastModifiedAt");

                            b1.HasKey("UserProfileId");

                            b1.ToTable("UserProfiles");

                            b1.WithOwner()
                                .HasForeignKey("UserProfileId");
                        });

                    b.OwnsOne("HolaViaje.Social.Shared.PlaceInfo", "Place", b1 =>
                        {
                            b1.Property<long>("UserProfileId")
                                .HasColumnType("bigint");

                            b1.Property<string>("City")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("Country");

                            b1.Property<string>("State")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("State");

                            b1.HasKey("UserProfileId");

                            b1.ToTable("UserProfiles");

                            b1.WithOwner()
                                .HasForeignKey("UserProfileId");

                            b1.OwnsOne("HolaViaje.Social.Shared.LocationInfo", "Location", b2 =>
                                {
                                    b2.Property<long>("PlaceInfoUserProfileId")
                                        .HasColumnType("bigint");

                                    b2.Property<string>("Latitude")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)")
                                        .HasColumnName("Latitude");

                                    b2.Property<string>("Longitude")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)")
                                        .HasColumnName("Longitude");

                                    b2.HasKey("PlaceInfoUserProfileId");

                                    b2.ToTable("UserProfiles");

                                    b2.WithOwner()
                                        .HasForeignKey("PlaceInfoUserProfileId");
                                });

                            b1.Navigation("Location");
                        });

                    b.Navigation("Availability")
                        .IsRequired();

                    b.Navigation("Control")
                        .IsRequired();

                    b.Navigation("Picture");

                    b.Navigation("Place")
                        .IsRequired();

                    b.Navigation("SpokenLanguages");
                });
#pragma warning restore 612, 618
        }
    }
}
