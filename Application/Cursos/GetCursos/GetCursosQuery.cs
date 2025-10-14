using System.Linq.Expressions;
using Application.Core;
using Application.Cursos.GetCurso;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Cursos.GetCursos
{
    public class GetCursosQuery
    {
        public record GetCursosQueryRequest : IRequest<Result<PaginatedList<CursoResponse>>>
        {
            public GetCursosRequest? CursosRequest { get; set; }
        }

        internal class
            GetCursosQueryHandler : IRequestHandler<GetCursosQueryRequest,
            Result<PaginatedList<CursoResponse>>>
        {
            private readonly ApisWebDbContext _context;

            public GetCursosQueryHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<PaginatedList<CursoResponse>>> Handle(
                GetCursosQueryRequest request,
                CancellationToken     cancellationToken)
            {
                IQueryable<Curso> queryable = _context.Cursos!.Include(x => x.Instructores)
                                                      .Include(x => x.Calificaciones)
                                                      .Include(x => x.Precios);

                Expression<Func<Curso, bool>> predicate = ExpressionBuilder.New<Curso>();
                if (!string.IsNullOrEmpty(request.CursosRequest!.Titulo))
                {
                    predicate = predicate.And(y => y.Titulo!.ToLower()
                                                    .Contains(request.CursosRequest.Titulo
                                                                  .ToLower()));
                }

                if (!string.IsNullOrEmpty(request.CursosRequest!.Descripcion))
                {
                    predicate = predicate.And(y => y.Descripcion!.ToLower()
                                                    .Contains(request.CursosRequest.Descripcion
                                                                  .ToLower()));
                }

                if (!string.IsNullOrEmpty(request.CursosRequest!.OrderBy))
                {
                    Expression<Func<Curso, object>> orderBySelector =
                        request.CursosRequest.OrderBy!.ToLower() switch
                        {
                            "titulo"      => curso => curso.Titulo!,
                            "descripcion" => curso => curso.Descripcion!,
                            _             => curso => curso.Titulo!
                        };

                    bool orderBy = request.CursosRequest.OrderAsc.HasValue
                                       ? request.CursosRequest.OrderAsc.Value
                                       : true;

                    queryable = orderBy
                                    ? queryable.OrderBy(orderBySelector)
                                    : queryable.OrderByDescending(orderBySelector);
                }

                queryable = queryable.Where(predicate);
                IQueryable<CursoResponse> cursosQuery =
                    queryable.ProjectToCursoResponse().AsQueryable();

                var pagination = await PaginatedList<CursoResponse>.CreateAsync(cursosQuery,
                                          request.CursosRequest.PageNumber,
                                          request.CursosRequest.PageSize);

                return Result<PaginatedList<CursoResponse>>.Success(pagination);
            }
        }
    }
}