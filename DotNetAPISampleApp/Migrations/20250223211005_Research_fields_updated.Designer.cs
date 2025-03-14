﻿// <auto-generated />
using DotNetAPISampleApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DotNetAPISampleApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250223211005_Research_fields_updated")]
    partial class Research_fields_updated
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("DotNetAPISampleApp.Models.Research.Research", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("finished")
                        .HasColumnType("INTEGER");

                    b.Property<string>("resultsSummary")
                        .HasColumnType("TEXT");

                    b.Property<string>("subject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("summary")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Researches");
                });
#pragma warning restore 612, 618
        }
    }
}
