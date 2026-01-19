using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.Repositories.Refunds;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Refund;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RefundController : ControllerBase
    {
        IRefundRepository _repository;

       
        public RefundController(IRefundRepository repository)
        {           
            _repository = repository;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<RefundInfo> List()
        {
            return _repository.List();
        }
        [HttpPost]
        [Route("SaveStatus")]
        public bool Save(SaveRefundStatusModel model)
        {
            return _repository.SaveStatus(model);
        }
        [HttpGet]
        [Route("StatusLogs")]
        public IEnumerable<RefundStatusInfo> StatusLogs(int refundRequestId)
        {
            return _repository.StatusLogs(refundRequestId);
        }
    }
}
