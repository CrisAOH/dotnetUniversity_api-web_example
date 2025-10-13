using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Cursos.CursoCreate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.Cursos.CursoCreate.CursoCreateCommand;
using static Application.Cursos.CursoReporteExcel.CursoReporteExcelQuery;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ISender _sender;
        public CursosController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Result<Guid>>> CursoCreate([FromForm] CursoCreateRequest request, CancellationToken cancellationToken)
        {
            CursoCreateCommandRequest command = new CursoCreateCommandRequest(request);

            return await _sender.Send(command, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ReporteCSV(CancellationToken cancellationToken)
        {
            //
            CursoReporteExcelQueryRequest query = new CursoReporteExcelQueryRequest();
            MemoryStream resultado = await _sender.Send(query, cancellationToken);
            byte[] excelBytes = resultado.ToArray();

            return File(excelBytes, "text/csv", "cursos.csv");
        }
    }
}