namespace Application.Instructores.GetInstructores
{
    public record InstructorResponse
    {
        public Guid ID { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public string Apellido { get; init; } = string.Empty;
        public string Grado { get; init; } = string.Empty;
    };
}