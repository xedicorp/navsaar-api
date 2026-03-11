using navsaar.api.Infrastructure;
using navsaar.api.ViewModels.Media;

namespace navsaar.api.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext _context;

        public MediaRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<MediaItemInfo> GetMediaItems()
        {
            return _context.MediaItems.Select(p => new MediaItemInfo
            {
                Id = p.Id,
                CreatedOn = DateTime.Now,
                Description = p.Description,
                IsDeleted = p.IsDeleted,
                MediaThumnailPath = p.MediaThumnailPath,
                MediaTypeId = p.MediaTypeId,
                MediaUrl = p.MediaUrl,
                Title = p.Title

            }).ToList();


        }
    }
}
