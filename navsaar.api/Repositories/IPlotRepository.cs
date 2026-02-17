using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;

namespace navsaar.api.Repositories
{
    public interface IPlotRepository
    {
        List<PlotInfo> List(int townshipId);
        PlotInfo GetById(int plotId);
        int Save(CreateEditPlotRequest request);
        bool HoldPlot(PlotHoldRequestModel model);
        void ReleaseExpiredHolds();
        List<HoldPlotInfo> GetHoldPlots(int townshipId);

    }
}
