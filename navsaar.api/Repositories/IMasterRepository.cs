

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


    }
}
