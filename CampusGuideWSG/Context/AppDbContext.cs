using System;
using System.Collections.Generic;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Entrance> Entrances { get; set; }

    public virtual DbSet<Moderator> Moderators { get; set; }

    public virtual DbSet<ModeratorBuilding> ModeratorsBuildings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("buildings");

            entity.HasIndex(e => e.Name, "BUILDINGS_NAME_IX").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(2)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Entrance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("entrances");

            entity.HasIndex(e => e.BuildingId, "ENTRANCES_BUILDINGS_FK");

            entity.HasIndex(e => e.Name, "ENTRANCES_NAME_IX").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuildingId)
                .HasColumnType("int(11)")
                .HasColumnName("building_id");
            entity.Property(e => e.IsOpen).HasColumnName("isOpen");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");

            entity.HasOne(d => d.Building).WithMany(p => p.Entrances)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("ENTRANCES_BUILDINGS_FK");
        });

        modelBuilder.Entity<Moderator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("moderators");

            entity.HasIndex(e => e.RoleId, "MODERATORS_ROLES_FK");

            entity.HasIndex(e => e.Username, "MODERATORS_USERNAME_IX").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(128)
                .HasColumnName("surname");
            entity.Property(e => e.Username)
                .HasMaxLength(128)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Moderators)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("MODERATORS_ROLES_FK");
        });

        modelBuilder.Entity<ModeratorBuilding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("moderators_buildings");

            entity.HasIndex(e => e.BuildingId, "MODERATORS_BUILDINGS_BUILDINGS_FK");

            entity.HasIndex(e => e.ModeratorId, "MODERATORS_BUILDINGS_MODERATORS_FK");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuildingId)
                .HasColumnType("int(11)")
                .HasColumnName("building_id");
            entity.Property(e => e.ModeratorId)
                .HasColumnType("int(11)")
                .HasColumnName("moderator_id");

            entity.HasOne(d => d.Building).WithMany(p => p.ModeratorsBuildings)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MODERATORS_BUILDINGS_BUILDINGS_FK");

            entity.HasOne(d => d.Moderator).WithMany(p => p.ModeratorsBuildings)
                .HasForeignKey(d => d.ModeratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MODERATORS_BUILDINGS_MODERATORS_FK");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rooms");

            entity.HasIndex(e => e.BuildingId, "ROOMS_BUILDINGS_FK");

            entity.HasIndex(e => e.Number, "ROOMS_NUMBER_IX").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuildingId)
                .HasColumnType("int(11)")
                .HasColumnName("building_id");
            entity.Property(e => e.Number)
                .HasColumnType("int(3)")
                .HasColumnName("number");

            entity.HasOne(d => d.Building).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("ROOMS_BUILDINGS_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
