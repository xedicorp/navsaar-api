

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Report;

namespace navsaar.api.Repositories
{
    public interface IReportRepository
    {
      List<TownshipCollectionModel>  TownshipCollectionSummaryReport(int townshipId = 0, int userId = 0);
        List<TownshipCollectionDetail> TownshipCollectionDetailReport(int townshipId = 0);
         TownshipHealthReportModel  TownshipHealthReport(int townshipId );
        List<PlotAvailabilityInfo> PlotAvailabilityReport(int townshipId = 0, int statusId = 0);
        List<ProgressSummaryCount> ProgressSummaryReport(int townshipId);
    }
}
