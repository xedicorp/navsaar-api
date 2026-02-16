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
        public IEnumerable<PlotInfo> List(int townshipId, int status=0) //0: all 1:available 2: booked
        {
            return _repository.List(townshipId, status);
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

        [HttpPost]
        [Route("Hold")]
        public IActionResult HoldPlot(PlotHoldRequestModel model)
        {
            var result = _repository.HoldPlot(model.PlotId, model.AssociateId);

            if (!result)
                return BadRequest("Plot is not available for hold");

            return Ok("Plot put on hold for 24 hours");
        }

    }
}
