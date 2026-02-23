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
        [HttpGet]
        [Route("TownshipCollectionDetailReport")]
        public IEnumerable<TownshipCollectionDetail> TownshipCollectionDetailReport(int townshipId = 0)
        {
            return _repository.TownshipCollectionDetailReport(townshipId);
        }
        [HttpGet]
        [Route("TownshipHealthReport")]
        public  TownshipHealthReportModel  TownshipHealthReport(int townshipId  )
        {
            return _repository.TownshipHealthReport(townshipId);
        }

        [HttpGet]
        [Route("plot-availability")]
        public IActionResult PlotAvailabilityReport(
        int townshipId = 0,
        int statusId = 0)
        {
            var data = _repository.PlotAvailabilityReport(townshipId, statusId);
            return Ok(data);
        }
    }
}
