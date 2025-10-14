namespace Application.Imagenes.GetImagen
{
    public record ImagenResponse
    {
        public Guid ID { get; init; }
        public string URL { get; init; } = string.Empty;
        public Guid CursoID { get; init; }
    };

    /*
        ASÍ LO HIZO EN EL VÍDEO, PODRÍAS PROBARLO A VER SI FUNCIONA. PERO ESTO LO HIZO PARA LOS CAMPOS QUE SON LIST<>
        public record ImagenResponse(Guid? Id, string Url, Guid? CursoId)
        {
            public ImagenResponse() : this(null, null, null)
            {
            }
        };
    */
}