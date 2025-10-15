using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Cursos.CursoDelete
{
    public class CursoDeleteCommand
    {
        public record CursoDeleteCommandRequest(
            Guid? CursoId) : IRequest<Result<Unit>>;

        internal class CursoDeleteCommandHandler :
            IRequestHandler<CursoDeleteCommandRequest, Result<Unit>>
        {
            private readonly ApisWebDbContext _context;

            public CursoDeleteCommandHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(CursoDeleteCommandRequest request,
                                                   CancellationToken         cancellationToken)
            {
                var curso =
                    await _context.Cursos!.FirstOrDefaultAsync(x => x.ID == request.CursoId);

                if (curso is null)
                {
                    return Result<Unit>.Failure("No se encontro el Curso");
                }

                _context.Cursos!.Remove(curso);
                var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
                return resultado
                           ? Result<Unit>.Success(Unit.Value)
                           : Result<Unit>.Failure("Error al eliminar el curso");
            }
        }
    }
}