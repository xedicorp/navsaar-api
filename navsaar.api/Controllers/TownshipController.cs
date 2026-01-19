using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TownshipController : ControllerBase
    {
        ITownshipRepository _repository;

        private readonly ILogger<WeatherForecastController> _logger;

        public TownshipController(ILogger<WeatherForecastController> logger,
               ITownshipRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<TownshipInfo> List(int userId=0)
        {
            return _repository.List(userId);
        }
        [HttpPost]
        [Route("Save")]
        public bool Save( TownshipCreateUpdateRequest request)
        {
          return  _repository.Save(request);
        }
    }
}
