using System;
using System.Collections.Generic;
using Bogus;
using Domain;
using Microsoft.AspNetCore.Identity;
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

            /*modelBuilder.Entity<Curso>().HasData(CargarDataMaster().Item1);
            modelBuilder.Entity<Precio>().HasData(CargarDataMaster().Item2);
            modelBuilder.Entity<Instructor>().HasData(CargarDataMaster().Item3);

            CargarDataSeguridad(modelBuilder);*/
        }

        private void CargarDataSeguridad(ModelBuilder modelBuilder)
        {
            //var adminId = Guid.NewGuid().ToString();
            //var clientId = Guid.NewGuid().ToString();

            var adminId = Guid.NewGuid().ToString();
            var clientId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminId,
                    Name = CustomRoles.ADMIN,
                    NormalizedName = CustomRoles.ADMIN
                }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = clientId,
                    Name = CustomRoles.CLIENT,
                    NormalizedName = CustomRoles.CLIENT
                }
            );

            modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 1,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.CURSO_READ,
                    RoleId = adminId
                },
                 new IdentityRoleClaim<string>
                 {
                     Id = 2,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.CURSO_UPDATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 3,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.CURSO_WRITE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 4,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.CURSO_DELETE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 5,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.INSTRUCTOR_CREATE,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 6,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.INSTRUCTOR_READ,
                     RoleId = adminId
                 },
                 new IdentityRoleClaim<string>
                 {
                     Id = 7,
                     ClaimType = CustomClaims.POLICIES,
                     ClaimValue = PolicyMaster.INSTRUCTOR_UPDATE,
                     RoleId = adminId
                 },
                new IdentityRoleClaim<string>
                {
                    Id = 8,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMENTARIO_READ,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 9,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMENTARIO_DELETE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 10,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMENTARIO_CREATE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 11,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.CURSO_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 12,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.INSTRUCTOR_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 13,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMENTARIO_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 14,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMENTARIO_CREATE,
                    RoleId = clientId
                }
            );

        }

        private Tuple<Curso[], Precio[], Instructor[]> CargarDataMaster()
        {
            var cursos = new List<Curso>();
            var faker = new Faker();

            for (var i = 1; i < 10; i++)
            {
                var cursoId = Guid.NewGuid();
                cursos.Add(
                    new Curso
                    {
                        ID = cursoId,
                        Descripcion = faker.Commerce.ProductDescription(),
                        Titulo = faker.Commerce.ProductName(),
                        FechaPublicacion = DateTime.UtcNow
                    }
                );
            }

            var precioId = Guid.NewGuid();
            var precio = new Precio
            {
                ID = precioId,
                PrecioActual = 10.0m,
                PrecioPromocion = 8.0m,
                Nombre = "Precio Regular"
            };
            var precios = new List<Precio>
        {
            precio
        };

            var fakerInstructor = new Faker<Instructor>()
                .RuleFor(t => t.ID, _ => Guid.NewGuid())
                .RuleFor(t => t.Nombre, f => f.Name.FirstName())
                .RuleFor(t => t.Apellidos, f => f.Name.LastName())
                .RuleFor(t => t.Grado, f => f.Name.JobTitle());

            var instructores = fakerInstructor.Generate(10);


            return Tuple.Create(cursos.ToArray(), precios.ToArray(), instructores.ToArray());
        }
    }


}