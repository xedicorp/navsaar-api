using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlotController : ControllerBase
    {
        IPlotRepository _repository;

        private readonly ILogger<WeatherForecastController> _logger;

        public PlotController(ILogger<WeatherForecastController> logger,
               IPlotRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<PlotInfo> List(int townshipId)
        {
            return _repository.List(townshipId);
        }
        
    }
}
