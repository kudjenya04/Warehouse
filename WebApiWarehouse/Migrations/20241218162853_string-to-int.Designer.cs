﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiWarehouse.Data;

#nullable disable

namespace WebApiWarehouse.Migrations
{
    [DbContext(typeof(WebApiWarwhouseDbContext))]
    [Migration("20241218162853_string-to-int")]
    partial class stringtoint
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApiWarehouse.Models.ActivityLog", b =>
                {
                    b.Property<int>("IdAl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_AL");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAl"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Details")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_id");

                    b.HasKey("IdAl");

                    b.HasIndex("UserId");

                    b.ToTable("ActivityLog", (string)null);
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Enterprise", b =>
                {
                    b.Property<int>("IdE")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_E");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdE"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NameE")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name_E");

                    b.HasKey("IdE");

                    b.ToTable("Enterprises");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Inventory", b =>
                {
                    b.Property<int>("IdI")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_I");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdI"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("Product_id");

                    b.Property<int>("QuantityI")
                        .HasColumnType("int")
                        .HasColumnName("Quantity_I");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int")
                        .HasColumnName("Warehouse_id");

                    b.HasKey("IdI");

                    b.HasIndex("ProductId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Inventory", (string)null);
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Order", b =>
                {
                    b.Property<int>("IdO")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_O");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdO"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int")
                        .HasColumnName("Client_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("Created_at");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("Product_id");

                    b.Property<int>("QuantityO")
                        .HasColumnType("int")
                        .HasColumnName("Quantity_O");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("Status_id");

                    b.HasKey("IdO");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProductId");

                    b.HasIndex("StatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Product", b =>
                {
                    b.Property<int>("IdP")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_P");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdP"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("EnterpriseId")
                        .HasColumnType("int")
                        .HasColumnName("Enterprise_id");

                    b.Property<string>("NameP")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name_P");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("Unit_price");

                    b.HasKey("IdP");

                    b.HasIndex("EnterpriseId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Status", b =>
                {
                    b.Property<int>("IdStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Status");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStatus"));

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Status_name");

                    b.HasKey("IdStatus");

                    b.ToTable("Status", (string)null);
                });

            modelBuilder.Entity("WebApiWarehouse.Models.User", b =>
                {
                    b.Property<int>("IdU")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_U");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdU"));

                    b.Property<bool>("Admin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("Admin");

                    b.Property<int>("EnterpriseId")
                        .HasColumnType("int")
                        .HasColumnName("Enterprise_id");

                    b.Property<string>("NameU")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name_U");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("IdU");

                    b.HasIndex("EnterpriseId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Warehouse", b =>
                {
                    b.Property<int>("IdW")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_W");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdW"));

                    b.Property<int>("EnterpriseId")
                        .HasColumnType("int")
                        .HasColumnName("Enterprise_id");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("NameW")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name_W");

                    b.HasKey("IdW");

                    b.HasIndex("EnterpriseId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.ActivityLog", b =>
                {
                    b.HasOne("WebApiWarehouse.Models.User", "User")
                        .WithMany("ActivityLogs")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_ActivityLog_Users");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Inventory", b =>
                {
                    b.HasOne("WebApiWarehouse.Models.Product", "Product")
                        .WithMany("Inventories")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_Inventory_Products");

                    b.HasOne("WebApiWarehouse.Models.Warehouse", "Warehouse")
                        .WithMany("Inventories")
                        .HasForeignKey("WarehouseId")
                        .IsRequired()
                        .HasConstraintName("FK_Inventory_Warehouses");

                    b.Navigation("Product");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Order", b =>
                {
                    b.HasOne("WebApiWarehouse.Models.User", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .IsRequired()
                        .HasConstraintName("FK_Orders_Users");

                    b.HasOne("WebApiWarehouse.Models.Product", "Product")
                        .WithMany("Order")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_Orders_Products");

                    b.HasOne("WebApiWarehouse.Models.Status", "Status")
                        .WithMany("Orders")
                        .HasForeignKey("StatusId")
                        .IsRequired()
                        .HasConstraintName("FK_Orders_Status");

                    b.Navigation("Client");

                    b.Navigation("Product");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Product", b =>
                {
                    b.HasOne("WebApiWarehouse.Models.Enterprise", "Enterprise")
                        .WithMany("Products")
                        .HasForeignKey("EnterpriseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.User", b =>
                {
                    b.HasOne("WebApiWarehouse.Models.Enterprise", "Enterprise")
                        .WithMany("Users")
                        .HasForeignKey("EnterpriseId")
                        .IsRequired()
                        .HasConstraintName("FK_Users_Enterprises");

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Warehouse", b =>
                {
                    b.HasOne("WebApiWarehouse.Models.Enterprise", "Enterprise")
                        .WithMany("Warehouses")
                        .HasForeignKey("EnterpriseId")
                        .IsRequired()
                        .HasConstraintName("FK_Warehouses_Enterprises");

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Enterprise", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Users");

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Product", b =>
                {
                    b.Navigation("Inventories");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Status", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.User", b =>
                {
                    b.Navigation("ActivityLogs");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("WebApiWarehouse.Models.Warehouse", b =>
                {
                    b.Navigation("Inventories");
                });
#pragma warning restore 612, 618
        }
    }
}
