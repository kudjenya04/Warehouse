using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApiWarehouse.Models;

namespace WebApiWarehouse.Data;

public partial class WebApiWarwhouseDbContext : DbContext
{
    public WebApiWarwhouseDbContext()
    {
    }

    public WebApiWarwhouseDbContext(DbContextOptions<WebApiWarwhouseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
    public virtual DbSet<Enterprise> Enterprises { get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Status> Statuses { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-D8M7KEN;Database=Sklad;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.IdAl);
            entity.ToTable("ActivityLog");
            entity.Property(e => e.IdAl)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_AL");
            entity.Property(e => e.Action)
                .HasMaxLength(255);
            entity.Property(e => e.Details)
                .HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasColumnName("User_id");
            entity.HasOne(d => d.User)
                .WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityLog_Users");
        });

        modelBuilder.Entity<Enterprise>(entity =>
        {
            entity.HasKey(e => e.IdE);
            entity.Property(e => e.IdE)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_E");
            entity.Property(e => e.Address)
                .HasMaxLength(255);
            entity.Property(e => e.Contact)
                .HasMaxLength(20);
            entity.Property(e => e.NameE)
                .HasMaxLength(100)
                .HasColumnName("Name_E");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.IdI);
            entity.ToTable("Inventory");
            entity.Property(e => e.IdI)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_I");
            entity.Property(e => e.ProductId)
                .HasColumnName("Product_id");
            entity.Property(e => e.QuantityI)
                .HasColumnName("Quantity_I");
            entity.Property(e => e.WarehouseId)
                .HasColumnName("Warehouse_id");
            entity.HasOne(d => d.Product)
                .WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Products");
            entity.HasOne(d => d.Warehouse)
                .WithMany(p => p.Inventories)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Warehouses");
        });


        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdO);

            entity.Property(e => e.IdO)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_O");
            entity.Property(e => e.ClientId)
                .HasColumnName("Client_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.StatusId)
        .HasColumnName("Status_id");
            entity.HasOne(d => d.Client)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
            entity.Property(e => e.ProductId)
             .HasColumnName("Product_id");
            entity.Property(e => e.QuantityO)
                .HasColumnName("Quantity_O");
            entity.HasOne(d => d.Status)
                .WithMany(p => p.Orders)
        .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Status");
            entity.HasOne(d => d.Product)
               .WithMany(p => p.Order)
               .HasForeignKey(d => d.ProductId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Orders_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdP);
            entity.Property(e => e.IdP)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_P");
            entity.Property(e => e.Description)
                .HasMaxLength(255);
            entity.Property(e => e.NameP)
                .HasMaxLength(100)
                .HasColumnName("Name_P");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Unit_price");
            entity.Property(e => e.EnterpriseId)
              .HasColumnName("Enterprise_id");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus);
            entity.ToTable("Status");
            entity.Property(e => e.IdStatus)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_Status");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("Status_name");
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdU);

            entity.Property(e => e.IdU)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_U");
            entity.Property(e => e.EnterpriseId)
                .HasColumnName("Enterprise_id");
            entity.Property(e => e.NameU)
                .HasMaxLength(100)
                .HasColumnName("Name_U");
            entity.Property(e => e.Password)
                .HasMaxLength(255);

            // Обновляем конфигурацию существующего столбца Admin
            entity.Property(e => e.Admin)
                .HasColumnName("Admin")
                .HasDefaultValue(false); // Указываем значение по умолчанию

            entity.HasOne(d => d.Enterprise)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.EnterpriseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Enterprises");
        });


        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.IdW);
            entity.Property(e => e.IdW)
                .ValueGeneratedOnAdd()  // Сделать поле автоинкрементным
                .HasColumnName("Id_W");
            entity.Property(e => e.EnterpriseId)
                .HasColumnName("Enterprise_id");
            entity.Property(e => e.Location)
                .HasMaxLength(255);
            entity.Property(e => e.NameW)
                .HasMaxLength(100)
                .HasColumnName("Name_W");

            entity.HasOne(d => d.Enterprise)
                .WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.EnterpriseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouses_Enterprises");
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
