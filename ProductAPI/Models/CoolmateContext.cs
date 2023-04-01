﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Models;

public partial class CoolmateContext : DbContext
{
    public CoolmateContext()
    {
    }

    public CoolmateContext(DbContextOptions<CoolmateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-E3SOURSA\\SQLEXPRESS;Database=Coolmate;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__23CAF1D8F8873ACE");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__Collecti__5BCE195C5CD551B8");

            entity.Property(e => e.CollectionId).HasColumnName("collectionId");
            entity.Property(e => e.CollectionName)
                .HasMaxLength(50)
                .HasColumnName("collectionName");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.Id }).HasName("PK__Colors__CE31EFE9E728F6DA");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ColorImg)
                .HasMaxLength(300)
                .HasColumnName("colorImg");
            entity.Property(e => e.ColorText)
                .HasMaxLength(20)
                .HasColumnName("colorText");

            entity.HasOne(d => d.Product).WithMany(p => p.Colors)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Colors__productI__412EB0B6");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.Id }).HasName("PK__Images__CE31EFE955F1164B");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("imgUrl");

            //entity.HasOne(d => d.Product).WithMany(p => p.Images)
            //    .HasForeignKey(d => d.ProductId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Images__productI__46E78A0C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F69846DFB");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CollectionId).HasColumnName("collectionId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.ProductTypeId).HasColumnName("productTypeId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.QuantityAvailable).HasColumnName("quantityAvailable");
            entity.Property(e => e.Sale)
                .HasMaxLength(255)
                .HasColumnName("sale");

            //entity.HasOne(d => d.Category).WithMany(p => p.Products)
            //    .HasForeignKey(d => d.CategoryId)
            //    .HasConstraintName("FK_Collections_Product");

            //entity.HasOne(d => d.Collection).WithMany(p => p.Products)
            //    .HasForeignKey(d => d.CollectionId)
            //    .HasConstraintName("FK_Categories_Product");

            //entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
            //    .HasForeignKey(d => d.ProductTypeId)
            //    .HasConstraintName("FK_Type_Product");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__ProductT__CA28F4DE0E6A66E3");

            entity.ToTable("ProductType");

            entity.Property(e => e.ProductTypeId).HasColumnName("productTypeId");
            entity.Property(e => e.Describe)
                .HasMaxLength(200)
                .HasColumnName("describe");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .HasColumnName("image");
            entity.Property(e => e.ProductTypeName)
                .HasMaxLength(200)
                .HasColumnName("productTypeName");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3213E83F6C795BCA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateReview)
                .HasColumnType("date")
                .HasColumnName("dateReview");
            entity.Property(e => e.Describe)
                .HasMaxLength(200)
                .HasColumnName("describe");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .HasColumnName("image");
            entity.Property(e => e.ProductTypeId).HasColumnName("productTypeId");
            entity.Property(e => e.Star).HasColumnName("star");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK__Reviews__product__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__userId__5070F446");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.Id }).HasName("PK__Sizes__CE31EFE9DF5297A1");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.QuantityAvalible).HasColumnName("quantityAvalible");
            entity.Property(e => e.SizeText)
                .HasMaxLength(10)
                .HasColumnName("sizeText");

            entity.HasOne(d => d.Product).WithMany(p => p.Sizes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sizes__productId__440B1D61");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F8FD5CBDA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}