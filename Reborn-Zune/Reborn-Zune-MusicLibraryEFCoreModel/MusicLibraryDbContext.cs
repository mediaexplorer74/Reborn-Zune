using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Reborn_Zune_MusicLibraryEFCoreModel
{
    public class MusicLibraryDbContext : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<MusicInPlaylist> MusicInPlaylists { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=library.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Music>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<string>(e => e.Path)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Path");

                entity.Property<string>(e => e.Title)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Title");

                entity.Property<string>(e => e.Duration)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Duration");

                entity.Property<string>(e => e.Artist)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Artist");

                entity.Property<string>(e => e.AlbumArtist)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("AlbumArtist");

                entity.Property<string>(e => e.Year)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Year");

                entity.Property<string>(e => e.ThumbnailId)
                    .IsUnicode()
                    .HasColumnName("ThumbnailId");

                entity.HasKey(a => a.Id);

                entity.HasIndex("Id")
                    .IsUnique()
                    .HasName("UQ__Music__AUQKR5RFNOUO6UFR");

                entity.ToTable("Music");

                entity.HasOne(m => m.Thumbnail)
                    .WithMany(t => t.Musics)
                    .HasForeignKey(t => t.ThumbnailId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Playlist>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<string>(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Name");

                entity.HasKey(e => e.Id);

                entity.HasIndex("Id")
                    .IsUnique()
                    .HasName("UQ__Playlist__AUQKR5RFNOUO6UFR");

                entity.ToTable("Playlst");
            });

            builder.Entity<MusicInPlaylist>(entity =>
            {
            entity.Property<string>(e => e.MusicId)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(450)
                .HasColumnName("MusicId");

            entity.Property<string>(e => e.PlaylistId)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(450)
                .HasColumnName("PlaylistId");

            entity.HasKey(e => new { e.MusicId, e.PlaylistId });

                entity.ToTable("MusicInPlaylist");

                entity.HasOne(mp => mp.Music)
                    .WithMany(m => m.MusicInPlaylists)
                    .HasForeignKey(mp => mp.MusicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mp => mp.Playlist)
                    .WithMany(p => p.MusicInPlaylists)
                    .HasForeignKey(mp => mp.PlaylistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Thumbnail>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<byte[]>(e => e.ImageBytes)
                    .IsRequired()
                    .HasColumnType("BLOB")
                    .HasColumnName("Image");

                entity.HasKey("Id");

                entity.HasIndex("Id")
                    .IsUnique()
                    .HasName("UQ__Thumbnail__AUQKR5RFNOUO6UFR");

                entity.ToTable("Thumbnail");

            });
        }
    }
    
}
