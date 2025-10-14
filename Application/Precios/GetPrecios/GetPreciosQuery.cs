using System.Linq.Expressions;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Precios.GetPrecios
{
    public class GetPreciosQuery
    {
        public record GetPreciosQueryRequest : IRequest<Result<PaginatedList<PrecioResponse>>>
        {
            public GetPreciosRequest? PreciosRequest { get; set; }
        }

        internal class GetPreciosQueryHandler :
            IRequestHandler<GetPreciosQueryRequest, Result<PaginatedList<PrecioResponse>>>
        {
            private readonly ApisWebDbContext _context;

            public GetPreciosQueryHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<PaginatedList<PrecioResponse>>> Handle(
                GetPreciosQueryRequest request, CancellationToken cancellationToken)
            {
                IQueryable<Precio> queryable = _context.Precios!;

                var predicate = ExpressionBuilder.New<Precio>();

                if (!string.IsNullOrEmpty(request.PreciosRequest!.Nombre))
                {
                    predicate =
                        predicate.And(y => y.Nombre!.Contains(request.PreciosRequest!.Nombre));
                }

                if (!string.IsNullOrEmpty(request.PreciosRequest!.OrderBy))
                {
                    Expression<Func<Precio, object>> orderSelector =
                        request.PreciosRequest.OrderBy.ToLower() switch
                        {
                            "nombre" => precio => precio.Nombre!,
                            "precio" => precio => precio.PrecioActual,
                            _        => precio => precio.Nombre!
                        };

                    bool orderBy = request.PreciosRequest.OrderAsc.HasValue
                                       ? request.PreciosRequest.OrderAsc.Value
                                       : true;

                    queryable = orderBy
                                    ? queryable.OrderBy(orderSelector)
                                    : queryable.OrderByDescending(orderSelector);
                }

                queryable = queryable.Where(predicate);
                var preciosQuery = queryable.ProjectToPrecioResponse().AsQueryable();

                var pagination =
                    await PaginatedList<PrecioResponse>.CreateAsync(preciosQuery,
                        request.PreciosRequest.PageNumber, request.PreciosRequest.PageSize);

                return Result<PaginatedList<PrecioResponse>>.Success(pagination);
            }
        }
    }

    public record PrecioResponse
    {
        public Guid    ID              { get; init; }
        public string  Nombre          { get; init; } = string.Empty;
        public decimal PrecioActual    { get; init; }
        public decimal PrecioPromocion { get; init; }
    };
}