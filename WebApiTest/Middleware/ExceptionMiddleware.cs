using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApiTest.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                var response = e switch
                {
                    ValidationException validationException => new AppException(
                        StatusCodes.Status400BadRequest,
                        "Error de validación",
                    validationException.Errors.ToArray()
                    /*string.Join(
                        ",",
                        validationException.Errors.Select(err => err.ErrorMessage)
                    )*/
                    //JsonConvert.SerializeObject(validationException.Errors.ToArray())

                    ),
                    _ => new AppException(
                        context.Response.StatusCode,
                        e.Message,
                        new
                        {
                            e.Source,
                            e.StackTrace,
                            e.InnerException?.Message
                        }
                        //e.StackTrace?.ToString()
                    )
                };

                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}