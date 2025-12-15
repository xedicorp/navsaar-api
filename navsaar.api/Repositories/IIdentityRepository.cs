

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Repositories
{
    public interface IIdentityRepository
    {
        List<UserInfo> List();
        LoginResponse Login(LoginRequest request);
    }
}
