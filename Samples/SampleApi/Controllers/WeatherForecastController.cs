using Microsoft.AspNetCore.Mvc;
using PipeFilterCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IEnumerable<IPipeAndFilterServiceBuild<WeatherForecast>> _mypipes;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEnumerable<IPipeAndFilterServiceBuild<WeatherForecast>> pipeAndFilter)
        {
            _logger = logger;
            _mypipes = pipeAndFilter;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellation)
        {
            var cid = Guid.NewGuid().ToString();

            var pipe1 = await _mypipes.First(x => x.ServiceId == "opc1")
                .Create()
                .Logger(_logger)
                .CorrelationId(cid)
                .Init(new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), Summary = "PipeAndFilter-Opc1", TemperatureC = 0 })
                .Run();

            var pipe2 = await _mypipes.First(x => x.ServiceId == "opc2")
                .Create()
                .Logger(_logger)
                .CorrelationId(cid)
                .Init(new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), Summary = "PipeAndFilter-Opc2", TemperatureC = 100 })
                .Run(cancellation);

            return new WeatherForecast[] { pipe1.Value!, pipe2.Value! };
        }
    }
}