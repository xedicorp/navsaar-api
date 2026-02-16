using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;

namespace navsaar.api.Repositories
{
    public interface IPlotRepository
    {
        public List<PlotInfo> List(int townshipId, int status = 0);
        PlotInfo  GetById(int plotId);
        int Save(CreateEditPlotRequest request);
        bool HoldPlot(int plotId, int associateId);
        void ReleaseExpiredHolds();
    }
}
