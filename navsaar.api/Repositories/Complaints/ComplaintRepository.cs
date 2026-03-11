

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
 

namespace navsaar.api.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _context;
        public ComplaintRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<ComplaintInfo> List()
        {
            var q = from p in _context.Complaints
                    join ct in _context.ComplaintTypes
                    on p.ComplaintTypeId equals ct.Id
                    join t in _context.Townships
                    on p.TownshipId equals t.Id
                    select new ComplaintInfo
                    {
                        Id = p.Id,
                        ComplaintTypeId = p.ComplaintTypeId,
                        ComplaintTypeName = ct.Name,
                        ImagePath = string.IsNullOrEmpty(p.ImagePath)
                        ? null
                        : "https://api.navsaargroup.com/Uploads/" + p.ImagePath,
                        Notes = p.Notes,
                        SentBy = p.SentBy,
                        SentOn = p.SentOn,
                        TownshipId = p.TownshipId,
                        TownshipName = t.Name,
                        Status = p.Status,
                        StatusText = p.Status == 1 ? "Pending" : "Completed"
                    };

            return q.OrderByDescending(x => x.SentOn).ToList();
        }
        public async Task<bool> Save(CreateUpdateComplaintModel model)
        {
            var entity = new Complaint();

            if (model.Id > 0)
            {
                entity = _context.Complaints.FirstOrDefault(p => p.Id == model.Id);

                if (entity == null)
                    return false;
            }

            entity.ComplaintTypeId = model.ComplaintTypeId;
            entity.Notes = model.Notes;
            entity.SentBy = model.SentBy;
            entity.TownshipId = model.TownshipId;
            entity.Status = 1; // Pending

            if (model.Id == 0)
                entity.SentOn = DateTime.Now;

            if (model.Id == 0)
                _context.Complaints.Add(entity);

            // Image Upload (same pattern as Receipt)
            if (model.Image != null && model.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + "_" + model.Image.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                entity.ImagePath = fileName;
            }

            _context.SaveChanges();

            return true;
        }
        public ComplaintInfo GetById(int id)
        {
            var q = from p in _context.Complaints
                    join ct in _context.ComplaintTypes
                    on p.ComplaintTypeId equals ct.Id
                    join t in _context.Townships
                    on p.TownshipId equals t.Id
                    where p.Id == id
                    select new ComplaintInfo
                    {
                        Id = p.Id,
                        ComplaintTypeId = p.ComplaintTypeId,
                        ComplaintTypeName = ct.Name,
                        ImagePath = string.IsNullOrEmpty(p.ImagePath)
                        ? null
                        : "https://api.navsaargroup.com/Uploads/" + p.ImagePath,
                        Notes = p.Notes,
                        SentBy = p.SentBy,
                        SentOn = p.SentOn,
                        TownshipId = p.TownshipId,
                        TownshipName = t.Name,
                        Status = p.Status,
                        StatusText = p.Status == 1 ? "Pending" : "Completed"
                    };

            return q.FirstOrDefault();
        }

        public bool MarkComplete(int id)
        {
            var entity = _context.Complaints.FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return false;

            entity.Status = 2;

            _context.SaveChanges();

            return true;
        }
        public List<ComplaintType> GetComplaintTypes()
        {
            return _context.ComplaintTypes
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}
