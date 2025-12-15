using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;

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
        [HttpGet]
        [Route("GetById")]
        public  PlotInfo  GetById(int plotId)
        {
            return _repository.GetById(plotId);
        }

        [HttpPost]
        [Route("Save")]
        public  int  Save(CreateEditPlotRequest request)
        {
            return _repository.Save(request);
        }
    }
}
