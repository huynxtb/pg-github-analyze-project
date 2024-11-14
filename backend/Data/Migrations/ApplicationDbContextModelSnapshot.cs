﻿// <auto-generated />
using System;
using GithubAnalyzeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GithubAnalyzeAPI.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GithubAnalyzeAPI.Models.Clone", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<long>("RepoId")
                        .HasColumnType("bigint");

                    b.Property<string>("Timestamp")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Uniques")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Clones");
                });

            modelBuilder.Entity("GithubAnalyzeAPI.Models.HistorySync", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("SyncAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("HistorySyncs");
                });

            modelBuilder.Entity("GithubAnalyzeAPI.Models.Repository", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HtmlUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("RepoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("GithubAnalyzeAPI.Models.View", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<long>("RepoId")
                        .HasColumnType("bigint");

                    b.Property<string>("Timestamp")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Uniques")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Views");
                });
#pragma warning restore 612, 618
        }
    }
}
