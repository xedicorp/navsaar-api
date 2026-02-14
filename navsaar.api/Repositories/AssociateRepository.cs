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
                    ReraNo = a.RERA,
                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved
                })
                .ToList();
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
                    ReraNo = a.RERA,
                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved
                })
                .FirstOrDefault();
        }
        //CREATE
        public long Create(CreateUpdateAssociateModel model)
        {
            var associate = new navsaar.api.Models.AssociateInfo
            {
                FirstName = model.FirstName,
                ContactNo = model.ContactNo,
                LeaderName = model.LeaderName,
                RERA = model.ReraNo,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Associates.Add(associate);
            _context.SaveChanges();

            return associate.ID;
        }

        //UPDATE
        public bool Update(CreateUpdateAssociateModel model)
        {
            var associate = _context.Associates.FirstOrDefault(x => x.ID == model.Id);
            if (associate == null)
                return false;

            if (model.FirstName != null)
                associate.FirstName = model.FirstName;

            if (model.ContactNo != null)
                associate.ContactNo = model.ContactNo;

            if (model.LeaderName != null)
                associate.LeaderName = model.LeaderName;

            if (model.ReraNo != null)
            associate.RERA = model.ReraNo;

            associate.IsActive = true;
            associate.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return true;
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
