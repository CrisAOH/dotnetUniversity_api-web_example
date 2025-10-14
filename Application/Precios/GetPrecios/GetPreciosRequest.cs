using Application.Core;

namespace Application.Precios.GetPrecios
{
    public class GetPreciosRequest : PaginationParams
    {
        public string? Nombre { get; set; }
    }
}