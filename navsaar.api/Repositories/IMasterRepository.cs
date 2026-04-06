

using navsaar.api.Models;
using navsaar.api.ViewModels;
 

namespace navsaar.api.Repositories
{
    public interface IMasterRepository
    {
        List<PlotType> PlotTypeList();
        List<FacingType> FacingTypeList();
        List<BookingStatusType> BookingStatusTypeList();
        AppSettingInfo GetAppSetting();
        List<AppSettingInfo> GetAllAppSettings();
        List<Bank> BankList();
        BankInfo GetBankById(int id);
        bool DeleteBank(int id);
        Task<bool> SaveBank(CreateUpdateBankModel model);
        List<PlotShape> PlotShapeList();
        List<MediaItemType> MediaItemTypeList();

    }
}
