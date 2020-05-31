using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAPITest.Models
{
    public partial class SecurityApplicationContext : DbContext
    {
        public SecurityApplicationContext(DbContextOptions<SecurityApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<RolHasMenu> RolHasMenu { get; set; }
        public virtual DbSet<TokenUsuario> TokenUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu)
                    .HasName("PK__Menu__4D7EA8E1B78A03AE");

                entity.Property(e => e.NomMenu)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Urlmenu)
                    .HasColumnName("URLMenu")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Rol__2A49584CB08E11D9");

                entity.Property(e => e.NomRol)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RolHasMenu>(entity =>
            {
                entity.HasKey(e => e.IdRolHasMenu)
                    .HasName("PK__RolHasMe__2219EB28A4CD023E");

                entity.HasOne(d => d.IdMenuNavigation)
                    .WithMany(p => p.RolHasMenu)
                    .HasForeignKey(d => d.IdMenu)
                    .HasConstraintName("FK__RolHasMen__IdMen__3B75D760");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolHasMenu)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__RolHasMen__IdMen__3A81B327");
            });

            modelBuilder.Entity<TokenUsuario>(entity =>
            {
                entity.HasKey(e => e.IdTokenUsuario)
                    .HasName("PK__TokenUsu__B88F3A7A1263EF46");

                entity.Property(e => e.FechaGeneracion).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TokenUsuario)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__TokenUsua__IdUsu__412EB0B6");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__5B65BF9769FE900A");

                entity.Property(e => e.NickName)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NomUsuario)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Usuario__IdRol__3E52440B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
