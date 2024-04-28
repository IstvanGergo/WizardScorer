using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WizardScorer.Models;

public partial class WizardContext : DbContext
{
    public WizardContext() : base()
    {
    }

    public WizardContext(DbContextOptions<WizardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=WizardScorer.db");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.Property(e => e.GameId).HasColumnName("GameID");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasIndex(e => e.PlayerName, "IX_Players_Player_Name").IsUnique();

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.PlayerName).HasColumnName("Player_Name");
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.Property(e => e.ScoreId).HasColumnName("ScoreID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.Score1).HasColumnName("Score");

            entity.HasOne(d => d.Game).WithMany(p => p.Scores)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Player).WithMany(p => p.Scores)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
