﻿// <auto-generated />
using System;
using Catalog.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.API.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20210206082038_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.HasSequence("author_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("book_format_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("book_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("genres_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("language_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("offer_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("publisher_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("seller_hilo")
                .IncrementsBy(10);

            modelBuilder.Entity("Catalog.API.Model.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("author_hilo");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("Catalog.API.Model.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("book_hilo");

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<float>("AvgCustomerRating")
                        .HasColumnType("real");

                    b.Property<int>("BookFormatId")
                        .HasColumnType("integer");

                    b.Property<string>("CountryOfOrigin")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Dimension")
                        .HasColumnType("text");

                    b.Property<int>("GenresId")
                        .HasColumnType("integer");

                    b.Property<string>("ISBN10No")
                        .HasColumnType("text");

                    b.Property<string>("ISBN13No")
                        .HasColumnType("text");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("NoOfPages")
                        .HasColumnType("integer");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PublisherId")
                        .HasColumnType("integer");

                    b.Property<int>("SellerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookFormatId");

                    b.HasIndex("GenresId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("OfferId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("SellerId");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("Catalog.API.Model.BookFormat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("book_format_hilo");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BookFormat");
                });

            modelBuilder.Entity("Catalog.API.Model.Genres", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("genres_hilo");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Catalog.API.Model.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("language_hilo");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("Catalog.API.Model.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("offer_hilo");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PercentOffer")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Offer");
                });

            modelBuilder.Entity("Catalog.API.Model.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("publisher_hilo");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Publisher");
                });

            modelBuilder.Entity("Catalog.API.Model.Seller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseHiLo("seller_hilo");

                    b.Property<int>("AvgRating")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Seller");
                });

            modelBuilder.Entity("Catalog.API.Model.Book", b =>
                {
                    b.HasOne("Catalog.API.Model.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Model.BookFormat", "BookFormat")
                        .WithMany()
                        .HasForeignKey("BookFormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Model.Genres", "Genres")
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Model.Language", "Langauge")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Model.Offer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Model.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.API.Model.Seller", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("BookFormat");

                    b.Navigation("Genres");

                    b.Navigation("Langauge");

                    b.Navigation("Offer");

                    b.Navigation("Publisher");

                    b.Navigation("Seller");
                });
#pragma warning restore 612, 618
        }
    }
}
