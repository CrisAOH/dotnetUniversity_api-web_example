using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Accounts;
using Application.Accounts.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Accounts.Login.LoginCommand;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ISender _sender;

        public AccountController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> Login([FromBody] LoginRequest request,
                                                       CancellationToken cancellationToken)
        {
            var command = new LoginCommandRequest(request);
            var resultado = await _sender.Send(command, cancellationToken);

            return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized();
        }
    }
}