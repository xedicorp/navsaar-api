using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;

namespace navsaar.api.Repositories
{
    public interface IPlotRepository
    {
        public List<PlotInfo> List(int townshipId);
        PlotInfo  GetById(int plotId);
        int Save(CreateEditPlotRequest request);
    }
}
