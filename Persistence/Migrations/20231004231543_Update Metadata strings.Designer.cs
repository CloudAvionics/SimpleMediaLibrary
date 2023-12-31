﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleLibrary.Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(SimpleLibraryDbContext))]
    [Migration("20231004231543_Update Metadata strings")]
    partial class UpdateMetadatastrings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Persistence.Model.AppConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConfigName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConfigValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AppConfigs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConfigName = "RecordingDir",
                            ConfigValue = "Recordings"
                        },
                        new
                        {
                            Id = 2,
                            ConfigName = "ApplicationName",
                            ConfigValue = "Simple Media Library"
                        },
                        new
                        {
                            Id = 3,
                            ConfigName = "CompanyName",
                            ConfigValue = "CloudAvionics"
                        },
                        new
                        {
                            Id = 4,
                            ConfigName = "MediaFileNamingConvention",
                            ConfigValue = "{station}_{genre}_{title}_{publishdate}_{publishtime}"
                        });
                });

            modelBuilder.Entity("Persistence.Model.MediaFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DeletedTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("FileSize")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("MetadataId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MetadataId");

                    b.ToTable("MediaFiles");
                });

            modelBuilder.Entity("Persistence.Model.MediaFileMetadata", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Frequency")
                        .HasColumnType("TEXT");

                    b.Property<string>("Genre")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PublishDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PublishTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Station")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MediaFilesMetadatas");
                });

            modelBuilder.Entity("Persistence.Model.MediaFile", b =>
                {
                    b.HasOne("Persistence.Model.MediaFileMetadata", "Metadata")
                        .WithMany()
                        .HasForeignKey("MetadataId");

                    b.Navigation("Metadata");
                });
#pragma warning restore 612, 618
        }
    }
}
