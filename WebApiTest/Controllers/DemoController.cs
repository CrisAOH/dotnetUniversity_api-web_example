using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApiTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public DemoController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet("[action]")]
        public string GetNombre()
        {
            return "Mi primer controlador.";
        }

        [HttpGet("[action]")]
        public IActionResult GetAmbiente()
        {
            var mensaje = _configuration.GetValue<string>("MiVariable");
            var ambiente = _environment.EnvironmentName;

            return Ok(new { Ambiente = ambiente, Mensaje = mensaje });
        }
    }
}