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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<WeatherForecastController> _logger;

        public MasterController(ILogger<WeatherForecastController> logger,
               IDocumentTypeRepository repository, IMasterRepository masterRepository  ,
               IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _masterRepository = masterRepository;
            _httpContextAccessor = httpContextAccessor;
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
        [HttpGet]
        [Route("AppSettings")]
        public ActionResult<IEnumerable<AppSettingInfo>> AppSettings()
        {
            // Extract tenant ID from header "x-tenant-id"
              var tenantname = _httpContextAccessor.HttpContext?.Request.Headers["Tenant-Host"].ToString();


            var settings = _masterRepository.GetAllAppSettings();
            settings = settings.Where(s => tenantname.Contains( s.TenantName.ToLower() )).ToList();

            if (settings == null)
                return Ok(new List<AppSettingInfo>()); 

            return Ok(settings );
        }
        [HttpGet]
        [Route("Banks")]
        public ActionResult<IEnumerable<Bank>> BankList()
        {
            var banks = _masterRepository.BankList();
            return Ok(banks);
        }

        [HttpGet]
        [Route("PlotShapes")]
        public ActionResult<IEnumerable<PlotShape>> PlotShapes()
        {
            var shapes = _masterRepository.PlotShapeList();
            return Ok(shapes);
        }

    }
}
