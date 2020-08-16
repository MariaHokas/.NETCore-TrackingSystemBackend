using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace timeTrackingSystemBackend.Entities
{
    public partial class WebApiDatabaseContext : DbContext
    {
        public WebApiDatabaseContext()
        {
        }

        public WebApiDatabaseContext(DbContextOptions<WebApiDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Luokat> Luokat { get; set; }
        public virtual DbSet<Tunnit> Tunnit { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:marianlopputyo.database.windows.net,1433;Initial Catalog=WebApiDatabase;Persist Security Info=False;User ID=marianlopputyo;Password=Perhonen1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Luokat>(entity =>
            {
                entity.HasKey(e => e.LuokkahuoneId);

                entity.Property(e => e.LuokkahuoneId).HasColumnName("LuokkahuoneID");

                entity.Property(e => e.LuokkaNimi)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Tunnit>(entity =>
            {
                entity.ToTable("tunnit");

                entity.HasIndex(e => e.TunnitId)
                    .HasName("IX_tunnit");

                entity.Property(e => e.TunnitId).HasColumnName("TunnitID");

                entity.Property(e => e.LuokkahuoneId)
                    .IsRequired()
                    .HasColumnName("LuokkahuoneID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.OppilasId)
                    .HasColumnName("OppilasID")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Sisaan).HasColumnType("datetime");

                entity.Property(e => e.Ulos).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
