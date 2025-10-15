namespace Application.Imagenes
{
    public class FotoUploadResult
    {
        public string? Url { get; set; }          // Ruta pública relativa o absoluta
        public string? PublicId { get; set; }     // Id único (aquí usamos el filename)
        public string? FileName { get; set; }
        public long Size { get; set; }
        public string? ContentType { get; set; }

    }
}