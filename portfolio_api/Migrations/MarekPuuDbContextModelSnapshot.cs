﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using portfolio_api.Data;

#nullable disable

namespace portfolio_api.Migrations
{
    [DbContext(typeof(MarekPuuDbContext))]
    partial class MarekPuuDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("portfolio_api.Data.AuthServerUser", b =>
                {
                    b.Property<string>("AuthServerUserId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Joined")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("AuthServerUserId");

                    b.ToTable("AuthServerUsers");

                    b.HasData(
                        new
                        {
                            AuthServerUserId = "google-oauth2|117233659145082710607",
                            Email = "marek.puurunen@gmail.com",
                            Joined = new DateTime(2023, 3, 28, 10, 1, 5, 578, DateTimeKind.Utc).AddTicks(528)
                        },
                        new
                        {
                            AuthServerUserId = "auth0|6416fc4ce1118da83ff07523",
                            Email = "marek2.puurunen@gmail.com",
                            Joined = new DateTime(2023, 3, 28, 10, 1, 5, 578, DateTimeKind.Utc).AddTicks(532)
                        });
                });

            modelBuilder.Entity("portfolio_api.Data.Household", b =>
                {
                    b.Property<Guid>("HouseholdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ownerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("HouseholdId");

                    b.HasIndex("ownerId");

                    b.ToTable("Households");
                });

            modelBuilder.Entity("portfolio_api.Data.HouseholdUser", b =>
                {
                    b.Property<Guid>("HouseholdId")
                        .HasColumnType("uuid");

                    b.Property<string>("AuthServerUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("MemberSince")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("HouseholdId", "AuthServerUserId");

                    b.HasIndex("AuthServerUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("HouseholdUsers");
                });

            modelBuilder.Entity("portfolio_api.Data.Role", b =>
                {
                    b.Property<int>("roleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("roleId"));

                    b.Property<string>("roleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("roleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            roleId = 1,
                            roleName = "Owner"
                        },
                        new
                        {
                            roleId = 2,
                            roleName = "Admin"
                        },
                        new
                        {
                            roleId = 3,
                            roleName = "User"
                        });
                });

            modelBuilder.Entity("portfolio_api.Data.Household", b =>
                {
                    b.HasOne("portfolio_api.Data.AuthServerUser", "Owner")
                        .WithMany()
                        .HasForeignKey("ownerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("portfolio_api.Data.HouseholdUser", b =>
                {
                    b.HasOne("portfolio_api.Data.AuthServerUser", "AuthServerUser")
                        .WithMany("HouseholdUsers")
                        .HasForeignKey("AuthServerUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("portfolio_api.Data.Household", "Household")
                        .WithMany("HouseholdUsers")
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("portfolio_api.Data.Role", "Role")
                        .WithMany("HouseholdUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuthServerUser");

                    b.Navigation("Household");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("portfolio_api.Data.AuthServerUser", b =>
                {
                    b.Navigation("HouseholdUsers");
                });

            modelBuilder.Entity("portfolio_api.Data.Household", b =>
                {
                    b.Navigation("HouseholdUsers");
                });

            modelBuilder.Entity("portfolio_api.Data.Role", b =>
                {
                    b.Navigation("HouseholdUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
