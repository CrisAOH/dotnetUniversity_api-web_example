using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Accounts;
using Application.Accounts.GetCurrentUser;
using Application.Accounts.Login;
using Application.Accounts.Register;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Accounts.Login.LoginCommand;
using static Application.Accounts.Register.RegisterCommand;
using static Application.Accounts.GetCurrentUser.GetCurrentUserQuery;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IUserAccessor _user;

        public AccountController(ISender sender, IUserAccessor user)
        {
            _sender = sender;
            _user = user;
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

        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommandRequest(request);
            var resultado = await _sender.Send(command, cancellationToken);

            return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized();
        }

        [Authorize]
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> Me(CancellationToken cancellationToken)
        {
            var email = _user.GetEmail();
            var request = new GetCurrentUserRequest { Email = email };
            var query = new GetCurrentUserQueryRequest(request);
            var resultado = await _sender.Send(query, cancellationToken);

            return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized();
        }
    }
}