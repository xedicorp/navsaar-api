

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Report;

namespace navsaar.api.Repositories
{
    public interface IReportRepository
    {
      List<TownshipCollectionModel>  TownshipCollectionSummaryReport(int townshipId = 0);
        List<TownshipCollectionDetail> TownshipCollectionDetailReport(int townshipId = 0);
         TownshipHealthReportModel  TownshipHealthReport(int townshipId );
    }
}
