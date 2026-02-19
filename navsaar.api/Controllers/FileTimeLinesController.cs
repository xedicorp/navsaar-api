using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels.FileTimelines;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileTimelineController : ControllerBase
    {
        private readonly IFileTimelinesRepository _repository;

        public FileTimelineController(IFileTimelinesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetList")]
        public List<FileTimelinesInfo> GetList()
        {
            return _repository.GetList();
        }

        [HttpGet("GetById/{id}")]
        public ActionResult<FileTimelinesInfo> GetById(int id)
        {
            var result = _repository.GetById(id);
            if (result == null)
                return NotFound("File timeline not found");

            return Ok(result);
        }

        [HttpPost("Save")]
        public async Task<ActionResult<bool>> Save(CreateUpdateFileTimelinesModel model)
        {
            var result = await _repository.Save(model);
            if (!result)
                return NotFound("File timeline not found");

            return Ok(true);
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var deleted = _repository.Delete(id);
            if (!deleted)
                return NotFound("File timeline not found");

            return Ok("File timeline deleted successfully");
        }
    }
}
