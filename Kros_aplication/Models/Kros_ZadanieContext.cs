using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kros_aplication.Models
{
    public partial class Kros_ZadanieContext : DbContext
    {
        public Kros_ZadanieContext()
        {
        }

        public Kros_ZadanieContext(DbContextOptions<Kros_ZadanieContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Division> Divisions { get; set; } = null!;
        public virtual DbSet<Firm> Firms { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Worker> Workers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdManager).HasColumnName("id_manager");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_4");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_6");
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.ToTable("Division");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirmId).HasColumnName("firm_id");

                entity.Property(e => e.IdManager).HasColumnName("id_manager");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.Divisions)
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_5");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.Divisions)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_2");
            });

            modelBuilder.Entity<Firm>(entity =>
            {
                entity.ToTable("Firm");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdManager).HasColumnName("id_manager");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.Firms)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_1");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DivisionId).HasColumnName("division_id");

                entity.Property(e => e.IdManager).HasColumnName("id_manager");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_7");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("R_3");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.ToTable("Worker");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
