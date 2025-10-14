using Application.Core;

namespace Application.Instructores.GetInstructores
{
    public class GetInstructoresRequest : PaginationParams
    {
        public string? Nombre   { get; set; }
        public string? Apellido { get; set; }
    }
}