

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
            var data = (from p in _context.Complaints
                        join ct in _context.ComplaintTypes
                        on p.ComplaintTypeId equals ct.Id
                        join t in _context.Townships
                        on p.TownshipId equals t.Id
                        join a in _context.Associates
                        on p.SentBy equals a.ID into aJoin
                        from a in aJoin.DefaultIfEmpty()

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
                            SentByName = a.FirstName,
                            SentOn = p.SentOn,
                            TownshipId = p.TownshipId,
                            TownshipName = t.Name,
                            Status = p.Status,
                            StatusText = p.Status == 1 ? "Pending"
                           : p.Status == 2 ? "Completed"
                           : p.Status == 3 ? "No Action Required"
                           : "",
                            CompletedOn = p.CompletedOn,
                            CompletedNotes = p.CompletedNotes,
                        })
                        .OrderByDescending(x => x.SentOn)
                        .ToList();

            var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            foreach (var item in data)
            {
                item.SentOn = TimeZoneInfo.ConvertTimeFromUtc(item.SentOn, istZone);

                if (item.CompletedOn.HasValue && item.Status == 3)
                    item.CompletedOn = TimeZoneInfo.ConvertTimeFromUtc(item.CompletedOn.Value, istZone);
            }

            return data;
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
                entity.SentOn = DateTime.UtcNow;

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
            var data = (from p in _context.Complaints
                        join ct in _context.ComplaintTypes
                        on p.ComplaintTypeId equals ct.Id
                        join t in _context.Townships
                        on p.TownshipId equals t.Id
                        join a in _context.Associates
                        on p.SentBy equals a.ID into aJoin
                        from a in aJoin.DefaultIfEmpty()

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
                            SentByName = a.FirstName,
                            SentOn = p.SentOn,
                            TownshipId = p.TownshipId,
                            TownshipName = t.Name,
                            Status = p.Status,
                            StatusText = p.Status == 1 ? "Pending"
                           : p.Status == 2 ? "Completed"
                           : p.Status == 3 ? "No Action Required"
                           : "",
                            CompletedOn = p.CompletedOn,
                            CompletedNotes = p.CompletedNotes,
                        }).FirstOrDefault();

            if (data != null)
            {
                var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                data.SentOn = TimeZoneInfo.ConvertTimeFromUtc(data.SentOn, istZone);

                if (data.CompletedOn.HasValue && data.Status == 3)
                    data.CompletedOn = TimeZoneInfo.ConvertTimeFromUtc(data.CompletedOn.Value, istZone);
            }

            return data;
        }
        public bool UpdateStatus(int id, int status, DateTime? completedOn, string completedNotes)
        {
            var entity = _context.Complaints.FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return false;

            if (status != 2 && status != 3)
                throw new Exception("Invalid status");

            entity.Status = status;

            if (status == 2) // Completed
            {
                entity.CompletedOn = completedOn;
                entity.CompletedNotes = completedNotes;
            }
            else if (status == 3) // No Action Required
            {
                entity.CompletedOn = DateTime.UtcNow;
                entity.CompletedNotes = null;
            }

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
