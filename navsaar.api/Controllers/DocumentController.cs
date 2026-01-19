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
        [Route("Download")]
        public async Task<IActionResult> Download(int id)
        {
            var doc = _repository.GetById(id);
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fullFilePath = Path.Combine(uploadsFolder, doc.FilePath);
            byte[] contents = System.IO.File.ReadAllBytes(fullFilePath);
            return File(contents, "application/force-download", doc.FilePath);

        }

        [HttpGet]
        [Route("GetAllByBookingId")]
        public List<DocumentModel> GetAllByBookingId(int bookingId)
        {
            return _repository.GetAllByBookingId(bookingId);
        } 
        
    }
}
