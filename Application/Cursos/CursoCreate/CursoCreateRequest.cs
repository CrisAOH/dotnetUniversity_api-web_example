//Esta es la data que debe de enviar el cliente
namespace Application.Cursos.CursoCreate
{
    public class CursoCreateRequest
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        //public IFormFile? Foto { get; set; }
        // En lugar de IFormFile, usa estos campos:
        public Stream? FotoStream { get; set; }
        public string? FotoNombre { get; set; }
        public string? FotoContentType { get; set; }
        public Guid? InstructorID { get; set; }
        public Guid? PrecioID { get; set; }
    }
}