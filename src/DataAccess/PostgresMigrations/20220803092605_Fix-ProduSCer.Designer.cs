﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(BGEContext))]
    [Migration("20220803092605_Fix-ProduSCer")]
    partial class FixProduSCer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BusinessLogic.Models.BoardGame", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<long>("MaxAge")
                        .HasColumnType("bigint");

                    b.Property<long>("MaxDuration")
                        .HasColumnType("bigint");

                    b.Property<long>("MaxPlayerNum")
                        .HasColumnType("bigint");

                    b.Property<long>("MinAge")
                        .HasColumnType("bigint");

                    b.Property<long>("MinDuration")
                        .HasColumnType("bigint");

                    b.Property<long>("MinPlayerNum")
                        .HasColumnType("bigint");

                    b.Property<string>("Producer")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Year")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("BusinessLogic.Models.BoardGameEvent", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<long>("Cost")
                        .HasColumnType("bigint");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<long>("Duration")
                        .HasColumnType("bigint");

                    b.Property<long>("OrganizerID")
                        .HasColumnType("bigint");

                    b.Property<bool>("Purchase")
                        .HasColumnType("boolean");

                    b.Property<TimeOnly>("RegistrationTime")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("VenueID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("OrganizerID");

                    b.HasIndex("VenueID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("BusinessLogic.Models.EventGame", b =>
                {
                    b.Property<long>("BoardGameID")
                        .HasColumnType("bigint");

                    b.Property<long>("BoardGameEventID")
                        .HasColumnType("bigint");

                    b.HasKey("BoardGameID", "BoardGameEventID");

                    b.HasIndex("BoardGameEventID");

                    b.ToTable("EventGameRelations");
                });

            modelBuilder.Entity("BusinessLogic.Models.FavoriteBoardGame", b =>
                {
                    b.Property<long>("BoardGameID")
                        .HasColumnType("bigint");

                    b.Property<long>("PlayerID")
                        .HasColumnType("bigint");

                    b.HasKey("BoardGameID", "PlayerID");

                    b.HasIndex("PlayerID");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("BusinessLogic.Models.Organizer", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("URL")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Organizers");
                });

            modelBuilder.Entity("BusinessLogic.Models.Player", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("League")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Rating")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("BusinessLogic.Models.PlayerRegistration", b =>
                {
                    b.Property<long>("BoardGameEventID")
                        .HasColumnType("bigint");

                    b.Property<long>("PlayerID")
                        .HasColumnType("bigint");

                    b.HasKey("BoardGameEventID", "PlayerID");

                    b.HasIndex("PlayerID");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("BusinessLogic.Models.Role", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<long>("RoleID")
                        .HasColumnType("bigint");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            ID = 1L,
                            RoleID = 0L,
                            RoleName = "guest",
                            UserID = 1L
                        });
                });

            modelBuilder.Entity("BusinessLogic.Models.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 1L,
                            Name = "guest",
                            Password = "guest"
                        });
                });

            modelBuilder.Entity("BusinessLogic.Models.Venue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("URL")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("BusinessLogic.Models.BoardGameEvent", b =>
                {
                    b.HasOne("BusinessLogic.Models.Organizer", "Organizer")
                        .WithMany()
                        .HasForeignKey("OrganizerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organizer");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("BusinessLogic.Models.EventGame", b =>
                {
                    b.HasOne("BusinessLogic.Models.BoardGameEvent", "BoardGameEvent")
                        .WithMany()
                        .HasForeignKey("BoardGameEventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Models.BoardGame", "BoardGame")
                        .WithMany()
                        .HasForeignKey("BoardGameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardGame");

                    b.Navigation("BoardGameEvent");
                });

            modelBuilder.Entity("BusinessLogic.Models.FavoriteBoardGame", b =>
                {
                    b.HasOne("BusinessLogic.Models.BoardGame", "BoardGame")
                        .WithMany()
                        .HasForeignKey("BoardGameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardGame");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("BusinessLogic.Models.PlayerRegistration", b =>
                {
                    b.HasOne("BusinessLogic.Models.BoardGameEvent", "BoardGameEvent")
                        .WithMany()
                        .HasForeignKey("BoardGameEventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessLogic.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardGameEvent");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("BusinessLogic.Models.Role", b =>
                {
                    b.HasOne("BusinessLogic.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
