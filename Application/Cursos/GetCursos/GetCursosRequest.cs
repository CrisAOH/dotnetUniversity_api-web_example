using Application.Core;

namespace Application.Cursos.GetCursos
{
    public class GetCursosRequest : PaginationParams
    {
        public string? Titulo      { get; set; }
        public string? Descripcion { get; set; }
    }
}