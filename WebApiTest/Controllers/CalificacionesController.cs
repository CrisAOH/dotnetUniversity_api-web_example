using System.Threading;
using System.Threading.Tasks;
using Application.Calificaciones.GetCalificaciones;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.Calificaciones.GetCalificaciones.GetCalificacionesQuery;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalificacionesController : ControllerBase
    {
        private readonly ISender _sender;

        public CalificacionesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetCalificaciones(
            [FromQuery] GetCalificacionesRequest request, CancellationToken cancellationToken)
        {
            var query = new GetCalificacionesQueryRequest()
            {
                CalificacionesRequest = request
            };

            var resultado = await _sender.Send(query, cancellationToken);
            return resultado.IsSuccess ? Ok(resultado) : NotFound();
        }
    }
}