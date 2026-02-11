using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels.Associate;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssociateController : ControllerBase
    {
        private readonly IAssociateRepository _associateRepository;

        public AssociateController(IAssociateRepository associateRepository)
        {
            _associateRepository = associateRepository;
        }

        [HttpGet]
        [Route("GetList")]
        public List<AssociateInfo> GetList()
        {
            return _associateRepository.GetList();
        }

        [HttpGet]
        [Route("GetByRera")]
        public ActionResult<AssociateInfo> GetByRera(string reraNo)
        {
            var result = _associateRepository.GetByRera(reraNo);

            if (result == null)
                return NotFound("Associate not found for given RERA No");

            return Ok(result);
        }
    }
}
