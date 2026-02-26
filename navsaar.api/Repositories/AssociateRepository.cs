using navsaar.api.Infrastructure;
using navsaar.api.ViewModels.Associate;

namespace navsaar.api.Repositories
{
    public class AssociateRepository : IAssociateRepository
    {
        private readonly AppDbContext _context;

        public AssociateRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<AssociateInfo> GetList()
        {
            return _context.Associates
                .Select(a => new AssociateInfo
                {
                    Id = a.ID,
                    UserName = a.UserName,
                    FirstName = a.FirstName,
                    ContactNo = a.ContactNo,     // can be NULL
                    LeaderName = a.LeaderName,   // can be NULL
                    LeaderContactNo = a.LeaderContactNo,
                    ReraNo = a.RERA,
                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved
                })
                .ToList();
        }
        public AssociateInfo GetById(long id)
        {
            return _context.Associates
                .Where(a => a.ID == id)
                .Select(a => new AssociateInfo
                {
                    Id = a.ID,
                    UserName = a.UserName,
                    FirstName = a.FirstName,
                    ContactNo = a.ContactNo,
                    LeaderName = a.LeaderName,
                    LeaderContactNo = a.LeaderContactNo,
                    ReraNo = a.RERA,

                    DOB = a.DOB,
                    AnniversaryDate = a.AnniversaryDate,

                    PANCardNo = a.PANCardNo,
                    AadhaarNo = a.AadhaarNo,
                    PassportNo = a.PassportNo,

                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved,

                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,

                    RERACertificateFile = a.RERACertificateFile,
                    PhotoFile = a.PhotoFile,
                    PassportFile = a.PassportFile,
                    BankDocumentFile = a.BankDocumentFile
                })
                .FirstOrDefault();
        }
        public AssociateInfo GetByRera(string reraNo)
        {
            return _context.Associates
                .Where(a => a.RERA == reraNo)
                .Select(a => new AssociateInfo
                {
                    Id = a.ID,
                    UserName = a.UserName,
                    FirstName = a.FirstName,
                    ContactNo = a.ContactNo,
                    LeaderName = a.LeaderName,
                    LeaderContactNo = a.LeaderContactNo,
                    ReraNo = a.RERA,
                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved
                })
                .FirstOrDefault();
        }
        public async Task<bool> Save(CreateUpdateAssociateModel model)
        {
            Models.AssociateInfo entity;

            if (model.Id > 0)
            {
                entity = _context.Associates.FirstOrDefault(x => x.ID == model.Id);
                if (entity == null)
                    return false;
            }
            else
            {
                entity = new Models.AssociateInfo
                {
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                _context.Associates.Add(entity);
            }

            entity.FirstName = model.FirstName;
            entity.DOB = model.DOB;
            entity.AnniversaryDate = model.AnniversaryDate;
            entity.ContactNo = model.ContactNo;
            entity.PANCardNo = model.PANNo;
            entity.AadhaarNo = model.AadhaarNo;
            entity.RERA = model.ReraNo;
            entity.PassportNo = model.PassportNo;
            entity.LeaderName = model.LeaderName;
            entity.LeaderContactNo = model.LeaderContactNo;
            entity.UpdatedAt = DateTime.Now;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Associates");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Upload Files
            if (model.ReraCertificateFile != null)
                entity.RERACertificateFile = await SaveFile(model.ReraCertificateFile, uploadsFolder);

            if (model.PhotoFile != null)
                entity.PhotoFile = await SaveFile(model.PhotoFile, uploadsFolder);

            if (model.PassportFile != null)
                entity.PassportFile = await SaveFile(model.PassportFile, uploadsFolder);

            if (model.BankDocumentFile != null)
                entity.BankDocumentFile = await SaveFile(model.BankDocumentFile, uploadsFolder);

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveFile(IFormFile file, string folderPath)
        {
            var fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }


        //DELETE
        public bool Delete(long id)
        {
            var associate = _context.Associates.FirstOrDefault(x => x.ID == id);
            if (associate == null)
                return false;

            _context.Associates.Remove(associate);
            _context.SaveChanges();

            return true;
        }
}
}
