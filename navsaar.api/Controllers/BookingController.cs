using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        IBookingRepository _bookingRepository;

        private readonly ILogger<WeatherForecastController> _logger;

        public BookingController(ILogger<WeatherForecastController> logger,
             IBookingRepository bookingRepository)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<BookingInfo> List()
        {
            return _bookingRepository.List();
        }
        [HttpPost]
        [Route("Save")]
        public void Save(CreateUpdateBookingModel model)
        {
              _bookingRepository.Save(model);
        }
        [HttpPost]
        [Route("UpdateInitialPayment")]
        public bool UpdateInitialPayment(UpdateInitialPaymentRequest request)
        {
            return _bookingRepository.UpdateInitialPayment(request);
        }

        [HttpPost]
        [Route("UploadLoanDocument")]
        public bool UpdateDraftPerparationStatus(UploadLoanDocumentRequest request)
        {
            return _bookingRepository.UploadLoanDocument(request);
        }
        [HttpGet]
        [Route("GetAllUploadedLoanDocuments")]
        public List<DocumentModel> GetAllUploadedLoanDocuments(int bookingId)
        {
            return _bookingRepository.GetAllUploadedLoanDocuments(bookingId);
        }

        [HttpPost]
        [Route("UpdateLoginStatus")]
        public bool UpdateLoginStatus(UpdateLoginStatusRequest request)
        {
            return _bookingRepository.UpdateLoginStatus(request);
        }
        [HttpPost]
        [Route("UpdateDraftPerparationStatus")]
        public bool UpdateDraftPerparationStatus(DraftPerparationStatusRequest request)
        {
            return _bookingRepository.UpdateDraftPerparationStatus(request);
        }
    }
}
