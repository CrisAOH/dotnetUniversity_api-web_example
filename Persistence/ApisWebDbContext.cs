using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence
{
    public class ApisWebDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Curso>? Cursos { get; set; }
        public DbSet<Instructor>? Instructores { get; set; }
        public DbSet<Precio>? Precios { get; set; }
        public DbSet<Calificacion>? Calificaciones { get; set; }

        public ApisWebDbContext()
        {

        }

        public ApisWebDbContext(DbContextOptions<ApisWebDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Curso>()
                .ToTable("Cursos");
            modelBuilder.Entity<Instructor>()
                .ToTable("Instructores");
            modelBuilder.Entity<CursoInstructor>()
                .ToTable("CursosInstructores");
            modelBuilder.Entity<Precio>()
                .ToTable("Precios");
            modelBuilder.Entity<CursoPrecio>()
                .ToTable("CursosPrecios");
            modelBuilder.Entity<Calificacion>()
                .ToTable("Calificaciones");
            modelBuilder.Entity<Foto>()
                .ToTable("Fotos");

            modelBuilder.Entity<Precio>()
                .Property(b => b.PrecioActual)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Precio>()
                .Property(b => b.PrecioPromocion)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Precio>()
                .Property(b => b.Nombre)
                .HasColumnType("VARCHAR")
                .HasMaxLength(250);

            modelBuilder.Entity<Curso>()
                .HasMany(m => m.Fotos)
                .WithOne(m => m.Curso)
                .HasForeignKey(m => m.CursoID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Curso>()
                .HasMany(m => m.Calificaciones)
                .WithOne(m => m.Curso)
                .HasForeignKey(m => m.CursoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Curso>()
                .HasMany(m => m.Precios)
                .WithMany(m => m.Cursos)
                .UsingEntity<CursoPrecio>(
                    j => j
                        .HasOne(p => p.Precio)
                        .WithMany(p => p.CursoPrecios)
                        .HasForeignKey(p => p.PrecioID),
                    j => j
                        .HasOne(p => p.Curso)
                        .WithMany(p => p.CursoPrecios)
                        .HasForeignKey(p => p.CursoID),
                    j =>
                    {
                        j.HasKey(t => new { t.PrecioID, t.CursoID });
                    }
                );
            modelBuilder.Entity<Curso>()
                .HasMany(m => m.Instructores)
                .WithMany(m => m.Cursos)
                .UsingEntity<CursoInstructor>(
                    j => j
                        .HasOne(p => p.Instructor)
                        .WithMany(p => p.CursoInstructores)
                        .HasForeignKey(p => p.InstructorID),
                    j => j
                        .HasOne(p => p.Curso)
                        .WithMany(p => p.CursoInstructores)
                        .HasForeignKey(P => P.CursoID),
                    j =>
                    {
                        j.HasKey(t => new { t.InstructorID, t.CursoID });
                    }
                );
        }
    }
}