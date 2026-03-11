using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels.Media;

namespace navsaar.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        IMediaRepository _repository;
        public MediaController(
               IMediaRepository repository)
        {

            _repository = repository;

        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<MediaItemInfo> List()
        {
            return _repository.GetMediaItems();
        }
    }
}
