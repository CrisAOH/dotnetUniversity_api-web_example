namespace Application.Calificaciones.GetCalificaciones
{
    public record CalificacionResponse
    {
        public string Alumno { get; init; } = string.Empty;
        public string Puntaje { get; init; } = string.Empty;
        public string Comentario { get; init; } = string.Empty;
        public string NombreCurso { get; init; } = string.Empty;
    };
}