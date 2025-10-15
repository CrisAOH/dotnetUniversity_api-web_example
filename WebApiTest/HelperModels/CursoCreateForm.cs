using System;
using Microsoft.AspNetCore.Http;

namespace WebApiTest.HelperModels
{
    public class CursoCreateForm
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? InstructorID { get; set; }
        public Guid? PrecioID { get; set; }

        public IFormFile? Foto { get; set; }
    }
}