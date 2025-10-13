using System;

namespace Domain
{
    public class Calificacion : BaseEntity
    {
        public Guid? CursoID { get; set; }
        public string? Alumno { get; set; }
        public int Puntaje { get; set; }
        public string? Comentario { get; set; }
        public Curso? Curso { get; set; }
    }
}