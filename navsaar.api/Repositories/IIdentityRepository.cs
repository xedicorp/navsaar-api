

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IIdentityRepository
    {
        List<UserInfo> List();
      //  bool Save(TownshipCreateUpdateRequest request);
    }
}
