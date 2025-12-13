using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Report;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        IReportRepository _repository;

      
        public ReportController(    IReportRepository repository)
        {
           
            _repository = repository;
        }

        [HttpGet]
        [Route("TownshipCollectionSummaryReport")]
        public IEnumerable<TownshipCollectionModel> TownshipCollectionSummaryReport(int townshipId=0)
        {
            return _repository.TownshipCollectionSummaryReport(townshipId);
        }
        
    }
}
