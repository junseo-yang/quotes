﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuotesWebAPI.Data;

#nullable disable

namespace QuotesWebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231203213615_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "d0049975-fac3-4da2-a9df-1cf1b8e5c3d7",
                            Name = "QuoteManager",
                            NormalizedName = "QUOTE_MANAGER"
                        },
                        new
                        {
                            Id = "c8837a6f-da35-4246-86c2-91434b1c9b88",
                            Name = "QuoteUser",
                            NormalizedName = "QUOTE_USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("QuotesWebAPI.Models.Quote", b =>
                {
                    b.Property<int>("QuoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuoteId"));

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("Like")
                        .HasColumnType("int");

                    b.HasKey("QuoteId");

                    b.ToTable("Quotes");

                    b.HasData(
                        new
                        {
                            QuoteId = 1,
                            Author = "Mark Twain",
                            Description = "Good friends, good books, and a sleepy conscience: this is the ideal life.",
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8221),
                            Like = 3
                        },
                        new
                        {
                            QuoteId = 2,
                            Author = "Pearl S. Buck",
                            Description = "Many people lose the small joys in the hope for the big happiness.",
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8225),
                            Like = 4
                        },
                        new
                        {
                            QuoteId = 3,
                            Author = "Leo Tolstoy",
                            Description = "A quiet secluded life in the country, with the possibility of being useful to people to whom it is easy to do good, and who are not accustomed to have it done to them; then work which one hopes may be of some use; then rest, nature, books, music, love for one's neighbor — such is my idea of happiness.",
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8229),
                            Like = 5
                        });
                });

            modelBuilder.Entity("QuotesWebAPI.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            TagId = 1,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8252),
                            Name = "books"
                        },
                        new
                        {
                            TagId = 2,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8255),
                            Name = "contentment"
                        },
                        new
                        {
                            TagId = 3,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8259),
                            Name = "friends"
                        },
                        new
                        {
                            TagId = 4,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8262),
                            Name = "friendship"
                        },
                        new
                        {
                            TagId = 5,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8265),
                            Name = "life"
                        },
                        new
                        {
                            TagId = 6,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8269),
                            Name = "conduct-of-life"
                        },
                        new
                        {
                            TagId = 7,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8272),
                            Name = "country"
                        },
                        new
                        {
                            TagId = 8,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8275),
                            Name = "happiness"
                        },
                        new
                        {
                            TagId = 9,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8278),
                            Name = "music"
                        },
                        new
                        {
                            TagId = 10,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8281),
                            Name = "nature"
                        },
                        new
                        {
                            TagId = 11,
                            LastModified = new DateTime(2023, 12, 3, 16, 36, 15, 774, DateTimeKind.Local).AddTicks(8284),
                            Name = "work"
                        });
                });

            modelBuilder.Entity("QuotesWebAPI.Models.TagAssignment", b =>
                {
                    b.Property<int>("QuoteId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("QuoteId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("TagAssignments");

                    b.HasData(
                        new
                        {
                            QuoteId = 1,
                            TagId = 1
                        },
                        new
                        {
                            QuoteId = 1,
                            TagId = 2
                        },
                        new
                        {
                            QuoteId = 1,
                            TagId = 3
                        },
                        new
                        {
                            QuoteId = 1,
                            TagId = 4
                        },
                        new
                        {
                            QuoteId = 1,
                            TagId = 5
                        },
                        new
                        {
                            QuoteId = 2,
                            TagId = 2
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 1
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 6
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 2
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 7
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 8
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 5
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 9
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 10
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 11
                        });
                });

            modelBuilder.Entity("QuotesWebAPI.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("QuotesWebAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("QuotesWebAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuotesWebAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("QuotesWebAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuotesWebAPI.Models.TagAssignment", b =>
                {
                    b.HasOne("QuotesWebAPI.Models.Quote", "Quote")
                        .WithMany("TagAssignments")
                        .HasForeignKey("QuoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuotesWebAPI.Models.Tag", "Tag")
                        .WithMany("TagAssignments")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quote");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("QuotesWebAPI.Models.Quote", b =>
                {
                    b.Navigation("TagAssignments");
                });

            modelBuilder.Entity("QuotesWebAPI.Models.Tag", b =>
                {
                    b.Navigation("TagAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
