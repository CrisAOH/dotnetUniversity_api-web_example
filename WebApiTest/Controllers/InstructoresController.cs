using System.Threading;
using System.Threading.Tasks;
using Application.Instructores.GetInstructores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.Instructores.GetInstructores.GetInstructoresQuery;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstructoresController : ControllerBase
    {
        private readonly ISender _sender;

        public InstructoresController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetInstructores([FromQuery] GetInstructoresRequest request,
                                                        CancellationToken cancellationToken)
        {
            var query = new GetInstructoresQueryRequest
            {
                InstructorRequest = request
            };

            var resultado = await _sender.Send(query, cancellationToken);
            return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
        }
    }
}