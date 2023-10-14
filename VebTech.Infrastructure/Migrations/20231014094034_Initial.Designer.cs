﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VebTech.Infrastructure.Context;

#nullable disable

namespace VebTech.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231014094034_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VebTech.Domain.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin",
                            Password = "$2a$11$0Z.4.eDrWBAs6UbGBA4ExOGEm.7x9j13zQ0ZbEsYf.L8uJe2Sr7m6"
                        });
                });

            modelBuilder.Entity("VebTech.Domain.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Role 1",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Role 2",
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "Role 3",
                            UserId = 1
                        },
                        new
                        {
                            Id = 4,
                            Name = "Role 1",
                            UserId = 2
                        },
                        new
                        {
                            Id = 5,
                            Name = "Role 2",
                            UserId = 2
                        },
                        new
                        {
                            Id = 6,
                            Name = "Role 1",
                            UserId = 3
                        },
                        new
                        {
                            Id = 7,
                            Name = "Role 4",
                            UserId = 3
                        },
                        new
                        {
                            Id = 8,
                            Name = "Role 5",
                            UserId = 3
                        },
                        new
                        {
                            Id = 9,
                            Name = "Role 6",
                            UserId = 3
                        },
                        new
                        {
                            Id = 10,
                            Name = "Role 7",
                            UserId = 4
                        },
                        new
                        {
                            Id = 11,
                            Name = "Role 8",
                            UserId = 4
                        },
                        new
                        {
                            Id = 12,
                            Name = "Role 3",
                            UserId = 5
                        },
                        new
                        {
                            Id = 13,
                            Name = "Role 8",
                            UserId = 5
                        },
                        new
                        {
                            Id = 14,
                            Name = "Role 9",
                            UserId = 5
                        },
                        new
                        {
                            Id = 15,
                            Name = "Role 10",
                            UserId = 5
                        },
                        new
                        {
                            Id = 16,
                            Name = "Role 12",
                            UserId = 6
                        },
                        new
                        {
                            Id = 17,
                            Name = "Role 11",
                            UserId = 6
                        },
                        new
                        {
                            Id = 18,
                            Name = "Role 18",
                            UserId = 6
                        });
                });

            modelBuilder.Entity("VebTech.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 25,
                            Email = "user1@example.com",
                            Name = "User 1"
                        },
                        new
                        {
                            Id = 2,
                            Age = 30,
                            Email = "user2@example.com",
                            Name = "User 2"
                        },
                        new
                        {
                            Id = 3,
                            Age = 35,
                            Email = "user3@example.com",
                            Name = "User 3"
                        },
                        new
                        {
                            Id = 4,
                            Age = 25,
                            Email = "user4@example.com",
                            Name = "User 4"
                        },
                        new
                        {
                            Id = 5,
                            Age = 45,
                            Email = "user5@example.com",
                            Name = "User 5"
                        },
                        new
                        {
                            Id = 6,
                            Age = 35,
                            Email = "user6@example.com",
                            Name = "User 6"
                        });
                });

            modelBuilder.Entity("VebTech.Domain.Models.Role", b =>
                {
                    b.HasOne("VebTech.Domain.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VebTech.Domain.Models.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
