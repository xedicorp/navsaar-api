using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Receipt;

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
        [HttpGet]
        [Route("ListByBookingId")]
        public IEnumerable<ReceiptInfo> List(int bookingId)
        {
            return _receiptRepository.ListByBookingId(bookingId);
        }
        [HttpPost]
        [Route("Save")]
        public async Task<bool> Save([FromForm] CreateUpdateReceiptModel model)
        {
            return await _receiptRepository.Save(model);
        }

        [HttpPost]
        [Route("SendVerifRequest")]
        public bool SendVerificationRequest(VerifRequest model)
        {
            return _receiptRepository.SendVerificationRequest(model);
        }
        [HttpGet]
        [Route("VerificationRequests")]
        public VerificationRequestApiResponse VerificationRequests()
        {
            return _receiptRepository.VerificationRequests();
        }
        [HttpPost]
        [Route("Verify")]
        public bool Verify(VerifReceiptRequest model)
        {
            return _receiptRepository.Verify(model);
        }
    }
}
