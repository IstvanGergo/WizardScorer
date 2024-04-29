using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WizardScorer.Models;

public partial class WizardContext : DbContext
{
    public WizardContext()
    {
    }

    public WizardContext(DbContextOptions<WizardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayersGame> PlayersGames { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource=WizardScorer.db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.Property(e => e.GameId).HasColumnName("GameID").ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasIndex(e => e.PlayerName, "IX_Players_Player_Name").IsUnique();

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID").ValueGeneratedOnAdd();
            entity.Property(e => e.PlayerName).HasColumnName("Player_Name");
        });

        modelBuilder.Entity<PlayersGame>(entity =>
        {
            entity.HasKey(e => e.GamePlayerId);

            entity.ToTable("Players_Games");

            entity.Property(e => e.GamePlayerId).HasColumnName("GamePlayerID").ValueGeneratedOnAdd();
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

            entity.HasOne(d => d.Game).WithMany(p => p.PlayersGames)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Player).WithMany(p => p.PlayersGames)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.GamePlayerId);

            entity.Property(e => e.GamePlayerId)
                .ValueGeneratedNever()
                .HasColumnName("GamePlayerID");
            entity.Property(e => e.Score1).HasColumnName("Score");

            entity.HasOne(d => d.GamePlayer).WithOne(p => p.Score)
                .HasForeignKey<Score>(d => d.GamePlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
