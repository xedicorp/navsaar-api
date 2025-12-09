using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IPlotRepository
    {
        public List<PlotInfo> List(int townshipId);
    }
}
