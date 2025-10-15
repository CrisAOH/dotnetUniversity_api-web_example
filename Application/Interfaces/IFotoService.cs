using Application.Imagenes;

namespace Application.Interfaces
{
    public interface IFotoService
    {
        Task<FotoUploadResult> AddFoto(Stream fotoStream,
        string? fotoNombre,
        string? fotoContentType);

        Task<string> DeleteFoto(string publicID);
    }
}