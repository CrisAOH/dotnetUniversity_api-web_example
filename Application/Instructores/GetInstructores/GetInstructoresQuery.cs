using System.Linq.Expressions;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Instructores.GetInstructores
{
    public class GetInstructoresQuery
    {
        public record GetInstructoresQueryRequest :
            IRequest<Result<PaginatedList<InstructorResponse>>>
        {
            public GetInstructoresRequest? InstructorRequest { get; set; }
        }

        internal class GetInstructoresQueryHandler : IRequestHandler<GetInstructoresQueryRequest,
            Result<PaginatedList<InstructorResponse>>>
        {
            private readonly ApisWebDbContext _context;

            public GetInstructoresQueryHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<PaginatedList<InstructorResponse>>> Handle(
                GetInstructoresQueryRequest request, CancellationToken cancellationToken)
            {
                IQueryable<Instructor> queryable = _context.Instructores!;

                var predicate = ExpressionBuilder.New<Instructor>();
                if (!string.IsNullOrEmpty(request.InstructorRequest!.Nombre))
                {
                    predicate =
                        predicate.And(y => y.Nombre!.Contains(request.InstructorRequest!.Nombre));
                }

                if (!string.IsNullOrEmpty(request.InstructorRequest!.Apellido))
                {
                    predicate =
                        predicate.And(y => y.Nombre!.Contains(request.InstructorRequest!.Apellido));
                }

                if (!string.IsNullOrEmpty(request.InstructorRequest.OrderBy))
                {
                    Expression<Func<Instructor, object>> orderBySelector =
                        request.InstructorRequest.OrderBy.ToLower() switch
                        {
                            "nombre"   => instructor => instructor.Nombre!,
                            "apellido" => instructor => instructor.Apellidos!,
                            _          => instructor => instructor.Nombre!
                        };

                    bool orderBy = request.InstructorRequest.OrderAsc.HasValue
                                       ? request.InstructorRequest.OrderAsc.Value
                                       : true;

                    queryable = orderBy
                                    ? queryable.OrderBy(orderBySelector)
                                    : queryable.OrderByDescending(orderBySelector);
                }

                queryable = queryable.Where(predicate);
                IQueryable<InstructorResponse> instructoresQuery =
                    queryable.ProjectToInstructorResponse().AsQueryable();

                var pagination =
                    await PaginatedList<InstructorResponse>.CreateAsync(instructoresQuery,
                             request.InstructorRequest.PageNumber,
                             request.InstructorRequest.PageSize);

                return Result<PaginatedList<InstructorResponse>>.Success(pagination);
            }
        }
    }

    public record InstructorResponse
    {
        public Guid   ID       { get; init; }
        public string Nombre   { get; init; } = string.Empty;
        public string Apellido { get; init; } = string.Empty;
        public string Grado    { get; init; } = string.Empty;
    };
}