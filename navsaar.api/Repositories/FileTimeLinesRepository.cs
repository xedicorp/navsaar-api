using navsaar.api.Infrastructure;
using navsaar.api.ViewModels.FileTimelines;

namespace navsaar.api.Repositories
{
    public class FileTimelinesRepository : IFileTimelinesRepository
    {
        private readonly AppDbContext _context;

        public FileTimelinesRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<FileTimelinesInfo> GetList()
        {
            return (
                from ft in _context.FileTimelines
                join bs in _context.BookingStatusTypes
                    on ft.StatusId equals bs.Id into statusJoin
                from bs in statusJoin.DefaultIfEmpty()   
                select new FileTimelinesInfo
                {
                    Id = ft.Id,
                    StatusId = ft.StatusId,
                    StatusName = bs != null ? bs.Name : null, 
                    Days = ft.Days,
                    WorkflowTypeId = ft.WorkflowTypeId
                }
            ).ToList();
        }


        public FileTimelinesInfo GetById(int id)
        {
            return (
                from ft in _context.FileTimelines
                join bs in _context.BookingStatusTypes
                    on ft.StatusId equals bs.Id into statusJoin
                from bs in statusJoin.DefaultIfEmpty()
                where ft.Id == id
                select new FileTimelinesInfo
                {
                    Id = ft.Id,
                    StatusId = ft.StatusId,
                    StatusName = bs != null ? bs.Name : null, 
                    Days = ft.Days,
                    WorkflowTypeId = ft.WorkflowTypeId
                }
            ).FirstOrDefault();
        }

        public async Task<bool> Save(CreateUpdateFileTimelinesModel model)
        {
            Models.FileTimeline entity;

            if (model.Id > 0)
            {
                entity = _context.FileTimelines.FirstOrDefault(x => x.Id == model.Id);
                if (entity == null)
                    return false;
            }
            else
            {
                entity = new Models.FileTimeline();
                _context.FileTimelines.Add(entity);
            }

            entity.StatusId = model.StatusId;
            entity.Days = model.Days;
            entity.WorkflowTypeId = model.WorkflowTypeId;

            await _context.SaveChangesAsync();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _context.FileTimelines.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return false;

            _context.FileTimelines.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}
