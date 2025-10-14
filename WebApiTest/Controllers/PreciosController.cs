﻿using System.Threading;
using System.Threading.Tasks;
using Application.Precios.GetPrecios;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.Precios.GetPrecios.GetPreciosQuery;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PreciosController : ControllerBase
    {
        private readonly ISender _sender;

        public PreciosController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPrecios([FromQuery] GetPreciosRequest request,
                                                   CancellationToken             cancellationToken)
        {
            var query = new GetPreciosQueryRequest()
            {
                PreciosRequest = request
            };

            var resultado = await _sender.Send(query, cancellationToken);
            return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
        }
    }
}