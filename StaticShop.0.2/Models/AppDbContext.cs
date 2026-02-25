using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StaticShop._0._2.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=staticshop;uid=root;pwd=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.24-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_item_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_item_ibfk_2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorites_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorites_ibfk_2");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("orders_ibfk_1");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("order_items_ibfk_2");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("order_items_ibfk_1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("products_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Balance).HasDefaultValueSql("'100000'");
            entity.Property(e => e.Role).HasDefaultValueSql("'user'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
