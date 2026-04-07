using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels.Media;

namespace navsaar.api.Controllers
{
    [Route("[controller]")]
    //[Authorize]
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
        [AllowAnonymous]
        [Route("List")]
        public IEnumerable<MediaItemInfo> List(int? mediaTypeId, int? townshipId)
        {
            return _repository.GetMediaItems(mediaTypeId, townshipId);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Get/{id}")]
        public IActionResult Get(int id)
        {
            var data = _repository.GetMediaItemById(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromForm] CreateUpdateMediaItemModel model)
        {
            var id = _repository.SaveMediaItem(model);

            return Ok(id);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repository.DeleteMediaItem(id);

            if (!result)
                return NotFound();

            return Ok(true);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("MediaItemTypes")]
        public IEnumerable<MediaItemTypeInfo> MediaItemTypes()
        {
            return _repository.GetMediaItemTypes();
        }
    }
}
