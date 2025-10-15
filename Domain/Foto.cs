using System;

namespace Domain
{
    public class Foto : BaseEntity
    {
        public Guid? CursoID { get; set; }
        public string? Url { get; set; }

        public Curso? Curso { get; set; }

        public string? PublicID { get; set; }
    }
}