﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillsWebApp.Data;

namespace SkillsWebApp.Migrations
{
    [DbContext(typeof(SkillContext))]
    [Migration("20180914074605_dbbb")]
    partial class dbbb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Moserware.Skills.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Moserware.Skills.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SkillsWebApp.Data.PlayerInt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName");

                    b.Property<double>("PartialPlayPercentage");

                    b.Property<double>("PartialUpdatePercentage");

                    b.Property<int?>("RatingId");

                    b.HasKey("Id");

                    b.HasIndex("RatingId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SkillsWebApp.Data.PlayerInt", b =>
                {
                    b.HasOne("Moserware.Skills.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");
                });
#pragma warning restore 612, 618
        }
    }
}
