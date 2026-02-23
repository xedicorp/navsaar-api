

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
 

namespace navsaar.api.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly AppDbContext _context;
        public MasterRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<FacingType> FacingTypeList()
        {
            return _context.FacingTypes.ToList();
        }

        public List<PlotType> PlotTypeList()
        {
            return _context.PlotTypes.ToList(); 
        }
        public List<BookingStatusType> BookingStatusTypeList()
        {
            return _context.BookingStatusTypes.ToList();
        }
        public AppSettingInfo GetAppSetting()
        {
            var setting = (from s in _context.AppSettings
                           select new AppSettingInfo
                           {
                               Id = s.Id,
                               LogoUrl = s.LogoUrl,
                               ApiUrl = s.ApiUrl,
                               ColorTheme = s.ColorTheme,
                               CompanyName = s.CompanyName,
                           }).FirstOrDefault();

            return setting;
        }
        public List<AppSettingInfo> GetAllAppSettings()
        {
            var settings = (from s in _context.AppSettings
                           select new AppSettingInfo
                           {
                               Id = s.Id,
                               LogoUrl = s.LogoUrl,
                               ApiUrl = s.ApiUrl,
                               ColorTheme = s.ColorTheme,
                               CompanyName = s.CompanyName,
                               TenantName = s.TenantName

                           }).ToList();

            return settings;
        }
        public List<string> GetAllTenantNames()
        {
            return _context.AppSettings.Select(s => s.TenantName).ToList();
        }

        public List<Bank> BankList()
        {
            return _context.Banks
                .OrderBy(b => b.Name)
                .ToList();
        }
    }
}
