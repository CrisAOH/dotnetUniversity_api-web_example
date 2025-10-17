using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Cursos.CursoUpdate
{
    public class CursoUpdateCommand
    {
        public record CursoUpdateCommandRequest(
            CursoUpdateRequest cursoUpdateRequest,
            Guid? CursoId) : IRequest<Result<Guid>>, ICommandBase;

        internal class CursoUpdateCommandHandler :
            IRequestHandler<CursoUpdateCommandRequest, Result<Guid>>
        {
            private readonly ApisWebDbContext _context;

            public CursoUpdateCommandHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Guid>> Handle(CursoUpdateCommandRequest request,
                                                   CancellationToken cancellationToken)
            {
                var cursoID = request.CursoId;
                var curso = await _context.Cursos!.FirstOrDefaultAsync(x => x.ID == cursoID);

                if (curso is null)
                {
                    return Result<Guid>.Failure("El curso no existe.");
                }

                curso.Descripcion = request.cursoUpdateRequest.Descripcion;
                curso.Titulo = request.cursoUpdateRequest.Titulo;
                curso.FechaPublicacion = request.cursoUpdateRequest.FechaPublicacion;

                _context.Entry(curso).State = EntityState.Modified;
                bool resultado = await _context.SaveChangesAsync() > 0;

                return resultado
                           ? Result<Guid>.Success(curso.ID)
                           : Result<Guid>.Failure("Error al actualizar el curso.");
            }
        }

        public class CursoUpdateCommandRequestValidator :
            AbstractValidator<CursoUpdateCommandRequest>
        {
            public CursoUpdateCommandRequestValidator()
            {
                RuleFor(x => x.cursoUpdateRequest).SetValidator(new CursoUpdateValidator());
                RuleFor(x => x.CursoId).NotNull();
            }
        }
    }
}