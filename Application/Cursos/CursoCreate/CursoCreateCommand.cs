//Command
//CommandHandler

using Application.Core;
using Application.Imagenes;
using Application.Interfaces;
using Domain;
using FluentValidation;
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
            private readonly IFotoService _fotoService;

            public CursoCreateCommandHandler(ApisWebDbContext context, IFotoService fotoService)
            {
                _context = context;
                _fotoService = fotoService;
            }

            public async Task<Result<Guid>> Handle(CursoCreateCommandRequest request, CancellationToken cancellationToken)
            {
                var cursoID = Guid.NewGuid();
                Curso curso = new Curso
                {
                    ID = cursoID,
                    Titulo = request.cursoCreateRequest.Titulo,
                    Descripcion = request.cursoCreateRequest.Descripcion,
                    FechaPublicacion = request.cursoCreateRequest.FechaPublicacion
                };

                if (request.cursoCreateRequest.FotoStream is not null)
                {
                    FotoUploadResult fotoUploadResult = await _fotoService.AddFoto(request.cursoCreateRequest.FotoStream, request.cursoCreateRequest.FotoNombre, request.cursoCreateRequest.FotoContentType);

                    var foto = new Foto
                    {
                        ID = Guid.NewGuid(),
                        Url = fotoUploadResult.Url,
                        PublicID = fotoUploadResult.PublicId,
                        CursoID = cursoID,
                    };

                    curso.Fotos = new List<Foto>
                    {
                        foto
                    };
                }

                _context.Add(curso);

                bool resultado = await _context.SaveChangesAsync(cancellationToken) > 0;

                return resultado ? Result<Guid>.Success(curso.ID) : Result<Guid>.Failure("No se pudo insertar el curso.");
            }
        }

        public class CursoCreateCommandRequestValidator : AbstractValidator<CursoCreateCommandRequest>
        {
            public CursoCreateCommandRequestValidator()
            {
                RuleFor(x => x.cursoCreateRequest).SetValidator(new CursoCreateValidator());
            }
        }
    }
}