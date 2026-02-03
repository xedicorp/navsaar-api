

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface ITownshipRepository
    {
        List<TownshipInfo> List(int userId=0);
        bool Save(TownshipCreateUpdateRequest request);
       Task< bool> UploadInventory(UploadInventoryRequestModel request);
    }
}
