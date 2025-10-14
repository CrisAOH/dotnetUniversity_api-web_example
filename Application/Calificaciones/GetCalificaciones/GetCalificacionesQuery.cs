using System.Linq.Expressions;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Calificaciones.GetCalificaciones
{
    public class GetCalificacionesQuery
    {
        public record GetCalificacionesQueryRequest :
            IRequest<Result<PaginatedList<CalificacionResponse>>>
        {
            public GetCalificacionesRequest? CalificacionesRequest { get; set; }
        }

        internal class GetCalificacionesQueryHandler : IRequestHandler<GetCalificacionesQueryRequest
          , Result<PaginatedList<CalificacionResponse>>>
        {
            private readonly ApisWebDbContext _context;

            public GetCalificacionesQueryHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<PaginatedList<CalificacionResponse>>> Handle(
                GetCalificacionesQueryRequest request, CancellationToken cancellationToken)
            {
                IQueryable<Calificacion> queryable = _context.Calificaciones!;

                var predicate = ExpressionBuilder.New<Calificacion>();
                if (!string.IsNullOrEmpty(request.CalificacionesRequest!.Alumno))
                {
                    predicate =
                        predicate.And(y => y.Alumno!.Contains(request.CalificacionesRequest
                                                                  .Alumno));
                }

                if (request.CalificacionesRequest.CursoId is not null)
                {
                    predicate =
                        predicate.And(y => y.CursoID == request.CalificacionesRequest.CursoId);
                }

                if (!string.IsNullOrEmpty(request.CalificacionesRequest.OrderBy))
                {
                    Expression<Func<Calificacion, object>> orderBySelector =
                        request.CalificacionesRequest.OrderBy.ToLower() switch
                        {
                            "alumno" => calificacion => calificacion.Alumno!,
                            "curso"  => calificacion => calificacion.Curso!,
                            _        => calificacion => calificacion.Alumno!
                        };

                    bool orderBy = request.CalificacionesRequest.OrderAsc.HasValue
                                       ? request.CalificacionesRequest.OrderAsc.Value
                                       : true;

                    queryable = orderBy
                                    ? queryable.OrderBy(orderBySelector)
                                    : queryable.OrderByDescending(orderBySelector);
                }

                queryable = queryable.Where(predicate);
                var calificacionQuery = queryable.ProjectToCalificacionResponse().AsQueryable();

                var pagination =
                    await PaginatedList<CalificacionResponse>.CreateAsync(calificacionQuery,
                             request.CalificacionesRequest.PageNumber,
                             request.CalificacionesRequest.PageSize);

                return Result<PaginatedList<CalificacionResponse>>.Success(pagination);
            }
        }
    }

    public record CalificacionResponse
    {
        public string Alumno      { get; init; } = string.Empty;
        public string Puntaje     { get; init; } = string.Empty;
        public string Comentario  { get; init; } = string.Empty;
        public string NombreCurso { get; init; } = string.Empty;
    };
}