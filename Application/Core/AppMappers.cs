using Application.Calificaciones.GetCalificaciones;
using Application.Cursos.GetCurso;
using Application.Imagenes.GetImagen;
using Application.Instructores.GetInstructores;
using Application.Precios.GetPrecios;
using Domain;
using Riok.Mapperly.Abstractions;

namespace Application.Core
{
    [Mapper]
    public static partial class AppMappers
    {
        // MapProperty: (sourceMemberPath, targetMemberName)
        [MapProperty(nameof(Curso.Fotos), nameof(CursoResponse.Imagenes))]
        [MapperIgnoreSource(nameof(Curso.CursoInstructores))]
        [MapperIgnoreSource(nameof(Curso.CursoPrecios))]
        public static partial CursoResponse CursoToCursoResponse(this Curso curso);

        [MapProperty(nameof(Foto.Url), nameof(ImagenResponse.URL))]
        [MapperIgnoreSource(nameof(Foto.PublicID))]
        [MapperIgnoreSource(nameof(Foto.Curso))]
        public static partial ImagenResponse MapToImagenResponse(this Foto foto);

        [MapperIgnoreSource(nameof(Precio.CursoPrecios))]
        [MapperIgnoreSource(nameof(Precio.Cursos))]
        public static partial PrecioResponse MapToPrecioResponse(this Precio precio);

        [MapProperty(nameof(Instructor.Apellidos), nameof(InstructorResponse.Apellido))]
        [MapperIgnoreSource(nameof(Instructor.Cursos))]
        [MapperIgnoreSource(nameof(Instructor.CursoInstructores))]
        public static partial InstructorResponse
            MapToInstructorResponse(this Instructor instructor);

        [MapProperty(nameof(Calificacion.Curso.Titulo), nameof(CalificacionResponse.NombreCurso))]
        [MapperIgnoreSource(nameof(Calificacion.CursoID))]
        [MapperIgnoreSource(nameof(Calificacion.ID))]
        public static partial CalificacionResponse MapToCalificacionResponse(
            this Calificacion calificacion);

        // IQueryable projection
        public static partial IQueryable<CursoResponse> ProjectToCursoResponse(
            this IQueryable<Curso> query);

        public static partial IQueryable<InstructorResponse> ProjectToInstructorResponse(
            this IQueryable<Instructor> query);

        public static partial IQueryable<PrecioResponse> ProjectToPrecioResponse(
            this IQueryable<Precio> query);

        public static partial IQueryable<CalificacionResponse> ProjectToCalificacionResponse(
            this IQueryable<Calificacion> query);
    }
}