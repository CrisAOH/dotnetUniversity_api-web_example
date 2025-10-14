namespace Application.Precios.GetPrecios
{
    public record PrecioResponse
    {
        public Guid ID { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public decimal PrecioActual { get; init; }
        public decimal PrecioPromocion { get; init; }
    };
}