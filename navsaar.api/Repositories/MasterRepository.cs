

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
        public BankInfo GetBankById(int id)
        {
            return (from b in _context.Banks
                    where b.Id == id
                    select new BankInfo
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).FirstOrDefault();
        }

        public async Task<bool> SaveBank(CreateUpdateBankModel model)
        {
            if (model.Id > 0)
            {
                // UPDATE
                var existing = await _context.Banks.FindAsync(model.Id);
                if (existing == null)
                    return false;

                existing.Name = model.Name;

                _context.Banks.Update(existing);
            }
            else
            {
                // CREATE
                var bank = new Bank
                {
                    Name = model.Name
                };

                await _context.Banks.AddAsync(bank);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public bool DeleteBank(int id)
        {
            var bank = _context.Banks.Find(id);
            if (bank == null)
                return false;

            _context.Banks.Remove(bank);
            _context.SaveChanges();
            return true;
        }

        public List<PlotShape> PlotShapeList()
        {
            return _context.PlotShapes
                .OrderBy(p => p.ShapeName)
                .ToList();
        }

        public List<MediaItemType> MediaItemTypeList()
        {
            return _context.MediaItemTypes
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
