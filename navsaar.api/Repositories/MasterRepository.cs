

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
    }
}
