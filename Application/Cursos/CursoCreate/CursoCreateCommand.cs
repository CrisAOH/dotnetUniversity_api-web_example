//Command
//CommandHandler

using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Cursos.CursoCreate
{
    public class CursoCreateCommand
    {
        //Los records son más útiles cuando sólo se quiere guardar datos en memoria
        public record CursoCreateCommandRequest(CursoCreateRequest cursoCreateRequest) : IRequest<Result<Guid>>
        {

        }

        internal class CursoCreateCommandHandler : IRequestHandler<CursoCreateCommandRequest, Result<Guid>>
        {
            private readonly ApisWebDbContext _context;

            public CursoCreateCommandHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Guid>> Handle(CursoCreateCommandRequest request, CancellationToken cancellationToken)
            {
                Curso curso = new Curso
                {
                    ID = Guid.NewGuid(),
                    Titulo = request.cursoCreateRequest.Titulo,
                    Descripcion = request.cursoCreateRequest.Descripcion,
                    FechaPublicacion = request.cursoCreateRequest.FechaPublicacion
                };

                _context.Add(curso);

                bool resultado = await _context.SaveChangesAsync(cancellationToken) > 0;

                return resultado ? Result<Guid>.Success(curso.ID) : Result<Guid>.Failure("No se pudo insertar el curso.");
            }
        }
    }
}