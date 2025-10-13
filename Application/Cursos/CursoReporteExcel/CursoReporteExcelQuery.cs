using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Cursos.CursoReporteExcel
{
    public class CursoReporteExcelQuery
    {
        public record CursoReporteExcelQueryRequest : IRequest<MemoryStream> { }

        internal class CursoReporteExcelQueryHandler : IRequestHandler<CursoReporteExcelQueryRequest, MemoryStream>
        {
            private readonly ApisWebDbContext _context;
            private readonly IReportService<Curso> _reporteService;

            public CursoReporteExcelQueryHandler(ApisWebDbContext context, IReportService<Curso> reporteService)
            {
                _context = context;
                _reporteService = reporteService;
            }

            public async Task<MemoryStream> Handle(CursoReporteExcelQueryRequest request, CancellationToken cancellationToken)
            {
                List<Curso> cursos = await _context.Cursos!.Take(10).Skip(0).ToListAsync();

                return await _reporteService.GetCSVReport(cursos);
            }
        }
    }
}