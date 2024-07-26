﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProductAPI.Database;

#nullable disable

namespace ProductAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240726234549_AddCategories")]
    partial class AddCategories
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProductAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ParentCode")
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("ProductAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("BeneficiaryName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Catcode")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Mcc")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Catcode");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("ProductAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.HasOne("ProductAPI.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("Catcode");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ProductAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
