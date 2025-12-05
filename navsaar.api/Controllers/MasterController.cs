using Microsoft.AspNetCore.Mvc;
using navsaar.api.Models;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MasterController : ControllerBase
    {
        IDocumentTypeRepository _repository;

        private readonly ILogger<WeatherForecastController> _logger;

        public MasterController(ILogger<WeatherForecastController> logger,
               IDocumentTypeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("DocumentTypes")]
        public IEnumerable<DocumentType> List()
        {
            return _repository.List();
        }
    }
}
