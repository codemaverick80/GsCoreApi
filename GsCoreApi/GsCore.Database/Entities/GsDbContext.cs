using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GsCore.Database.Entities
{
    public class GsDbContext : DbContext
    {
       
        public GsDbContext(DbContextOptions<GsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<ArtistBasicInfo> ArtistBasicInfo { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartDetail> CartDetail { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Track> Track { get; set; }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GsDbContext>
        {
            public GsDbContext CreateDbContext(string[] args)
            {
                ////Microsoft.Extensions.Configuration.FileExtensions
                ////Microsoft.Extensions.Configuration.Json
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../GsCore.Api/appsettings.json")
                    .Build();
                var builder = new DbContextOptionsBuilder<GsDbContext>();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");
                builder.UseSqlServer(connectionString);
                return new GsDbContext(builder.Options);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AlbumName).HasMaxLength(200);

                entity.Property(e => e.AlbumUrl).HasMaxLength(500);

                entity.Property(e => e.Label).HasMaxLength(200);

                entity.Property(e => e.LargeThumbnail).HasMaxLength(50);

                entity.Property(e => e.MediumThumbnail).HasMaxLength(50);

                entity.Property(e => e.SmallThumbnail).HasMaxLength(50);

                entity.Property(e => e.ThumbnailTag).HasMaxLength(15);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("FK_Album_ToArtist");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_Album_ToGenre");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ArtistName).HasMaxLength(100);

                entity.Property(e => e.LargeThumbnail).HasMaxLength(50);

                entity.Property(e => e.SmallThumbnail).HasMaxLength(50);

                entity.Property(e => e.ThumbnailTag).HasMaxLength(20);

                entity.Property(e => e.YearActive).HasMaxLength(50);
            });

            modelBuilder.Entity<ArtistBasicInfo>(entity =>
            {
                entity.HasKey(e => e.ArtistId);
                
                entity.Property(e => e.AlsoKnownAs).HasMaxLength(500);

                entity.Property(e => e.Born).HasMaxLength(100);

                entity.Property(e => e.Died).HasMaxLength(100);

                entity.HasOne(d => d.Artist)
                    .WithOne(p => p.ArtistBasicInfo)
                    .HasForeignKey<ArtistBasicInfo>(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtistBasicInfo_Artist");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateCreated).HasColumnType("datetime");
            });

            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartDetail)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_CartDetail_ToCart");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_Inventory_ToAlbum");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.AddressLine1).HasMaxLength(100);

                entity.Property(e => e.AddressLine2).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.PostalCode).HasMaxLength(12);

                entity.Property(e => e.StateProvince).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK_OrderDetail_ToInventory");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetail_ToOrder");
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Duration).HasMaxLength(20);

                entity.Property(e => e.TrackName).HasMaxLength(400);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Track)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_Track_ToAlbum");
            });
        }
    }
}
