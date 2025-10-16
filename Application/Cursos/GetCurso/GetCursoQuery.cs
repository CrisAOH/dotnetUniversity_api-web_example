using Application.Calificaciones.GetCalificaciones;
using Application.Core;
using Application.Imagenes.GetImagen;
using Application.Instructores.GetInstructores;
using Application.Precios.GetPrecios;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Cursos.GetCurso
{
    public class GetCursoQuery
    {
        public record GetCursoQueryRequest : IRequest<Result<CursoResponse>>
        {
            //ESTO ES UN PARÁMETRO
            public Guid ID { get; set; }
        }

        internal class GetCursoQueryHandler : IRequestHandler<GetCursoQueryRequest, Result<CursoResponse>>
        {
            private readonly ApisWebDbContext _context;

            public GetCursoQueryHandler(ApisWebDbContext context)
            {
                _context = context;
            }

            public async Task<Result<CursoResponse>> Handle(GetCursoQueryRequest request, CancellationToken cancellationToken)
            {
                var curso = await _context.Cursos!.Where(x => x.ID == request.ID)
                .Include(x => x.Instructores)
                .Include(x => x.Precios)
                .Include(x => x.Calificaciones)
                .Include(x => x.Fotos)
                .ProjectToCursoResponse()
                .FirstOrDefaultAsync();

                return Result<CursoResponse>.Success(curso!);
            }
        }
    }

    public record CursoResponse
    {
        public Guid ID { get; init; }
        public string Titulo { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public DateTime? FechaPublicacion { get; init; }
        public List<InstructorResponse> Instructores { get; init; } = new();
        public List<CalificacionResponse> Calificaciones { get; init; } = new();
        public List<PrecioResponse> Precios { get; init; } = new();
        public List<ImagenResponse> Imagenes { get; init; } = new();
    }
}