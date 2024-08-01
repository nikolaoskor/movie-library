#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieLibrary.Models;

public partial class moviesLibraryDB : DbContext
{
    public moviesLibraryDB()
    {
    }

    public moviesLibraryDB(DbContextOptions<moviesLibraryDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Actors> Actors { get; set; }

    public virtual DbSet<Availabilities> Availabilities { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Movies> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
       => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=moviesLibraryDB;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actors>(entity =>
        {
            entity.HasKey(e => e.ActorId).HasName("PK__Actors__57B3EA2B2A96CBD3");

            entity.Property(e => e.ActorId).HasColumnName("ActorID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Availabilities>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PK__Availabi__DA3979918337385D");

            entity.Property(e => e.AvailabilityId).HasColumnName("AvailabilityID");
            entity.Property(e => e.AvailabilityName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B8F6434B7");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Movies>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2943A3D2BE7F6");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Imdbrating).HasColumnName("IMDBRating");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Watched).HasDefaultValueSql("((0))");
            entity.Property(e => e.WatchedDate).HasColumnType("date");

            entity.HasOne(d => d.Category).WithMany(p => p.Movies)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movies__Category__3D5E1FD2");

            entity.HasMany(d => d.Actor).WithMany(p => p.Movie)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieActors",
                    r => r.HasOne<Actors>().WithMany()
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MovieActo__Actor__4316F928"),
                    l => l.HasOne<Movies>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MovieActo__Movie__4222D4EF"),
                    j =>
                    {
                        j.HasKey("MovieId", "ActorId").HasName("PK__MovieAct__EEA9AA984D237C12");
                        j.IndexerProperty<int>("MovieId").HasColumnName("MovieID");
                        j.IndexerProperty<int>("ActorId").HasColumnName("ActorID");
                    });

            entity.HasMany(d => d.Availability).WithMany(p => p.Movie)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieAvailabilities",
                    r => r.HasOne<Availabilities>().WithMany()
                        .HasForeignKey("AvailabilityId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MovieAvai__Avail__46E78A0C"),
                    l => l.HasOne<Movies>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MovieAvai__Movie__45F365D3"),
                    j =>
                    {
                        j.HasKey("MovieId", "AvailabilityId").HasName("PK__MovieAva__467103A369D51E6C");
                        j.IndexerProperty<int>("MovieId").HasColumnName("MovieID");
                        j.IndexerProperty<int>("AvailabilityId").HasColumnName("AvailabilityID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}