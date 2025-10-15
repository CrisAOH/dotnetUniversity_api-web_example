using Application.Imagenes;
using Application.Interfaces;

namespace Infrastructure.Fotos
{
    public class FotoService : IFotoService
    {
        private readonly string _uploadsFolder;
        private readonly long _maxBytes = 5 * 1024 * 1024;

        public FotoService(string uploadsFolder)
        {
            _uploadsFolder = uploadsFolder;
        }

        private static readonly Dictionary<string, string> ContentTypeToExt = new()
        {
            {"image/jpeg", ".jpg"},
            {"image/jpg", ".jpg"},
            {"image/png", ".png"},
            {"image/webp", ".webp"}
        };

        public async Task<FotoUploadResult> AddFoto(Stream fotoStream, string? fotoNombre, string? fotoContentType)
        {
            if (fotoStream == null)
            {
                throw new Exception("Stream nulo");
            }

            if (fotoStream.Length == 0)
            {
                throw new Exception("Stream vacío.");
            }

            if (fotoStream.Length > _maxBytes)
            {
                throw new Exception($"El archivo excede el tamaño máximo de {_maxBytes} bytes");
            }

            if (!ContentTypeToExt.TryGetValue(fotoContentType?.ToLowerInvariant() ?? string.Empty, out string? ext))
            {
                throw new Exception("Tipo de contenido no permitido.");
            }

            try
            {
                if (!Directory.Exists(_uploadsFolder))
                {
                    Directory.CreateDirectory(_uploadsFolder);
                }

                // Generar nombre seguro (GUID + extensión)
                var publicId = Guid.NewGuid().ToString("N");
                var safeFileName = publicId + ext;
                var filePath = Path.Combine(_uploadsFolder, safeFileName);

                if (fotoStream.CanSeek)
                {
                    fotoStream.Position = 0;
                }

                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true))
                {
                    await fotoStream.CopyToAsync(fs);
                }

                // Construir URL pública relativa (usualmente /uploads/{file})
                var publicUrl = $"/uploads/{safeFileName}";

                var fileInfo = new FileInfo(filePath);

                return new FotoUploadResult
                {
                    Url = publicUrl,
                    PublicId = publicId,
                    FileName = safeFileName,
                    Size = fileInfo.Length,
                    ContentType = fotoContentType
                };
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<string> DeleteFoto(string publicID)
        {
            if (string.IsNullOrEmpty(publicID))
            {
                return "ID inválido";
            }

            try
            {
                // Busca cualquier archivo con el publicId (cualquier extensión)
                var files = Directory.GetFiles(_uploadsFolder, $"{publicID}.*");

                if (files.Length == 0)
                {
                    return "No se encontró el archivo a eliminar.";
                }

                foreach (var file in files)
                {
                    File.Delete(file);
                }

                return "Foto eliminada correctamente.";
            }
            catch (Exception e)
            {

                return $"Error al eliminar la foto: {e.Message}";
            }
        }
    }
}