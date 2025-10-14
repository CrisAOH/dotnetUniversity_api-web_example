using Application.Core;

namespace Application.Calificaciones.GetCalificaciones
{
    public class GetCalificacionesRequest : PaginationParams
    {
        public string? Alumno  { get; set; }
        public Guid?   CursoId { get; set; }
    }
}