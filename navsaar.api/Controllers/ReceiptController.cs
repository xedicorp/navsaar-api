using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiptController : ControllerBase
    {
        IReceiptRepository _receiptRepository;

        private readonly ILogger<WeatherForecastController> _logger;

        public ReceiptController(ILogger<WeatherForecastController> logger,
             IReceiptRepository receiptRepository)
        {
            _logger = logger;
            _receiptRepository = receiptRepository;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<ReceiptInfo> List()
        {
            return _receiptRepository.List();
        }
        [HttpPost]
        [Route("Save")]
        public bool Save(  CreateUpdateReceiptModel model)
        {
            return _receiptRepository.Save(model);
        } 
    }
}
