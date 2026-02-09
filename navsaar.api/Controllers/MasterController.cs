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
        IMasterRepository _masterRepository;

        private readonly ILogger<WeatherForecastController> _logger;

        public MasterController(ILogger<WeatherForecastController> logger,
               IDocumentTypeRepository repository, IMasterRepository masterRepository)
        {
            _logger = logger;
            _repository = repository;
            _masterRepository = masterRepository;
        }

        [HttpGet]
        [Route("DocumentTypes")]
        public IEnumerable<DocumentType> List()
        {
            return _repository.List();
        }
        [HttpGet]
        [Route("PlotTypes")]
        public IEnumerable<PlotType> PlotTypeList()
        {
            return _masterRepository.PlotTypeList();
        }
        [HttpGet]
        [Route("FacingTypes")]
        public IEnumerable<FacingType> FacingTypes()
        {
            return _masterRepository.FacingTypeList();
        }
        [HttpGet]
        [Route("BookingStatusTypes")]
        public IEnumerable<BookingStatusType> BookingStatusTypes()
        {
            return _masterRepository.BookingStatusTypeList();
        }
    }
}
