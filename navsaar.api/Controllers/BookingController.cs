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
        [HttpGet]
        [Route("GetById")]
        public  BookingInfo GetById(int bookingId)
        {
            return _bookingRepository.GetById(bookingId);
        }
        [HttpPost]
        [Route("Save")]
        public void Save([FromForm] CreateUpdateBookingModel model)
        {
              _bookingRepository.Save(model);
        }
        [HttpGet]
        [Route("GetBookingProgress")]
        public IEnumerable<BookingProgressModel> List(int bookingId)
        {
            return _bookingRepository.GetBookingProgress(bookingId);
        }

        [HttpPost]
        [Route("UpdateInitialPayment")]
        public bool UpdateInitialPayment(UpdateInitialPaymentRequest request)
        {
            return _bookingRepository.UpdateInitialPayment(request);
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
        [HttpPost]
        [Route("UpdateLoanSanctionStatus")]
        public bool UpdateLoanSanctionStatus(UpdateLoanSanctionStatusRequest request)
        {
            return _bookingRepository.UpdateLoanSanctionStatus(request);
        }
        [HttpPost]
        [Route("UpdateMarkFileCheckStatus")]
        public bool UpdateMarkFileCheckStatus(UpdateMarkFileCheckStatusRequest request)
        {
            return _bookingRepository.UpdateMarkFileCheckStatus(request);
        }
        [HttpPost]
        [Route("UploadOriginalATTRequest")]
        public void UploadOriginalATT([FromForm] UploadOriginalATTRequest request)
        {
            _bookingRepository.UploadOriginalATT(request);
        }
        [HttpPost]
        [Route("UpdateDokitSigningStatus")]
        public void UpdateDokitSigningStatus(  UpdateDokitSigningStatusRequest request)
        {
            _bookingRepository.UpdateDokitSigningStatus(request);
        }
        [HttpPost]
        [Route("UploadBankDD")]
        public void UploadBankDD([FromForm] UploadBankDDRequest request)
        {
            _bookingRepository.UploadBankDD(request);
        }
        [HttpPost]
        [Route("UpdateJDAPattaStatus")]
        public void UpdateJDAPattaStatus(  UpdateJDAPattaStatusRequest request)
        {
            _bookingRepository.UpdateJDAPattaStatus(request);
        }
        [HttpPost]
        [Route("UpdateBankDDStatus")]
        public void UpdateBankDDStatus( UpdateBankDDStatusRequest request)
        {
            _bookingRepository.UpdateBankDDStatus(request);
        }
    }
}
