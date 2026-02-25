using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Booking;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        IBookingRepository _bookingRepository;

      
        public BookingController( 
             IBookingRepository bookingRepository)
        {
           
            _bookingRepository = bookingRepository;
        }
        [HttpGet]
        [Route("Search")]
        public IEnumerable<BookingInfo> Search(int? statusTypeId , int? townshipId,int? bookingType, string? reraNo, DateTime? fromDate )
        {
            return _bookingRepository.Search(statusTypeId, townshipId,   bookingType, reraNo,   fromDate);
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
        public async Task<int> Save([FromForm] CreateUpdateBookingModel model)
        {
            return  await _bookingRepository.Save(model);
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
        [HttpPost]
        [Route("ChangePlot")]
        public bool ChangePlot(ChangePlotRequest request)
        {
           return  _bookingRepository.ChangePlot(request);
        }
        [HttpPost]
        [Route("Cancel")]
        public bool Cancel(BookingCancelRequest request)
        {
            return _bookingRepository.Cancel(request);
        }

        [HttpPost]
        [Route("SendToDraft")]
        public bool SendToDraft(  SendToDraftRequest request)
        {
            return _bookingRepository.SendToDraft(request);
        }
        [HttpGet]
        [Route("GetDraftRequests")]
        public DraftRequestApiResponse GetDraftRequests()
        {
            return _bookingRepository.GetDraftRequests();
        }
        [HttpPost]
        [Route("MarkDraftComplete")]
        public bool MarkDraftComplete(MarkDraftCompleteRequest request)
        {
            return _bookingRepository.MarkDraftComplete(request);
        }
        [HttpPost]
        [Route("SendForAllotmentLetter")]
        public bool SendForAllotmentLetter(  SendForAllotmentLetterRequestModel request)
        {
            return _bookingRepository.SendForAllotmentLetter(request);
        }
        [HttpGet]
        [Route("GetAllotmentLetterRequests")]
        public List<AllotmentLetterRequestInfo> GetAllotmentLetterRequests()
        {
            return _bookingRepository.GetAllotmentLetterRequests();
        }
        [HttpPost]
        [Route("MarkAllotmentLetterComplete")]
        public bool MarkAllotmentLetterComplete(MarkAllotmentLetterCompleteRequest request)
        {
            return _bookingRepository.MarkAllotmentLetterComplete(request);
        }
        [HttpGet]
        [Route("GetCheckList")]
        public List<DocumentModel> GetCheckList(int bookingId)
        {
           
            return _bookingRepository.GetCheckList(bookingId);
        }
    }
}
