using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Cursos.CursoCreate;
using Application.Cursos.GetCurso;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.Cursos.CursoCreate.CursoCreateCommand;
using static Application.Cursos.CursoReporteExcel.CursoReporteExcelQuery;
using static Application.Cursos.GetCurso.GetCursoQuery;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IValidator<CursoCreateRequest> _validator;

        public CursosController(ISender sender, IValidator<CursoCreateRequest> validator)
        {
            _sender = sender;
            _validator = validator;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Result<Guid>>> CursoCreate([FromForm] CursoCreateRequest request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            CursoCreateCommandRequest command = new CursoCreateCommandRequest(request);

            return await _sender.Send(command, cancellationToken);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCurso(Guid ID, CancellationToken cancellationToken)
        {
            GetCursoQueryRequest query = new GetCursoQueryRequest { ID = ID };

            Result<CursoResponse> result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest();
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