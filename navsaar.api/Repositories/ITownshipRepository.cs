

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface ITownshipRepository
    {
        List<TownshipInfo> List();
        bool Save(TownshipCreateUpdateRequest request);
    }
}
