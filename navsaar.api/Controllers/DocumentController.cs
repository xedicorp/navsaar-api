using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        IDocumentRepository _repository;

        private readonly ILogger<WeatherForecastController> _logger;

        public DocumentController(ILogger<WeatherForecastController> logger,
             IDocumentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        } 

        [HttpPost]
        [Route("Upload")]
        public  async Task<bool> Upload([FromForm] UploadDocumentRequest request)
        {
            return await _repository.Upload(request);
        }
        [HttpGet]
        [Route("GetAllByBookingId")]
        public List<DocumentModel> GetAllByBookingId(int bookingId)
        {
            return _repository.GetAllByBookingId(bookingId);
        } 
        
    }
}
