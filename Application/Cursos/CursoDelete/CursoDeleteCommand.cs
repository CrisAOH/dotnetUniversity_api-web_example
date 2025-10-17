using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Cursos.CursoDelete
{
    public class CursoDeleteCommand
    {
        public record CursoDeleteCommandRequest(
            Guid? CursoId) : IRequest<Result<Unit>>, ICommandBase;

        internal class CursoDeleteCommandHandler :
            IRequestHandler<CursoDeleteCommandRequest, Result<Unit>>
        {
            private readonly ApisWebDbContext _context;

            public CursoDeleteCommandHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(CursoDeleteCommandRequest request,
                                                   CancellationToken cancellationToken)
            {
                //SE ELIMINARÁN LAS RELACIONES EN LA TABLA INTERMEDIA, NO EN LAS TABLAS DE CADA UNA.
                var curso =
                    await _context.Cursos!
                    .Include(x => x.Instructores)
                    .Include(x => x.Precios)
                    .Include(x => x.Calificaciones)
                    .Include(x => x.Fotos)
                    .FirstOrDefaultAsync(x => x.ID == request.CursoId);

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

        public class CursoDeleteCommandRequestValidator : AbstractValidator<CursoDeleteCommandRequest>
        {
            public CursoDeleteCommandRequestValidator()
            {
                RuleFor(x => x.CursoId).NotNull().WithMessage("No se ha enviado el ID del curso.");
            }
        }
    }
}