using System;

namespace Domain
{
    public class CursoPrecio
    {
        public Guid? CursoID { get; set; }
        public Curso? Curso { get; set; }

        public Guid? PrecioID { get; set; }
        public Precio? Precio { get; set; }
    }
}