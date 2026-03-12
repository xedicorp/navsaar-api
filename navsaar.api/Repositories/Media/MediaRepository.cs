using navsaar.api.Infrastructure;
using navsaar.api.Models;
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
            var data = _context.MediaItems
                .Where(p => p.IsDeleted != true)
                .Select(p => new MediaItemInfo
                {
                    Id = p.Id,
                    CreatedOn = p.CreatedOn,
                    Description = p.Description,
                    IsDeleted = p.IsDeleted,
                    MediaThumnailPath = string.IsNullOrEmpty(p.MediaThumnailPath)
                        ? null
                        : "https://api.navsaargroup.com/Uploads/Media/" + p.MediaThumnailPath,
                    MediaTypeId = p.MediaTypeId,
                    MediaUrl = p.MediaUrl,
                    Title = p.Title
                })
                .ToList();

            var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            foreach (var item in data)
            {
                item.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(item.CreatedOn, istZone);
            }

            return data;
        }

        public MediaItemInfo? GetMediaItemById(int id)
        {
            var data = _context.MediaItems
                .Where(p => p.Id == id && p.IsDeleted != true)
                .Select(p => new MediaItemInfo
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    MediaTypeId = p.MediaTypeId,
                    MediaUrl = p.MediaUrl,
                    MediaThumnailPath = string.IsNullOrEmpty(p.MediaThumnailPath)
                        ? null
                        : "https://api.navsaargroup.com/Uploads/Media/" + p.MediaThumnailPath,                    IsDeleted = p.IsDeleted,
                    CreatedOn = p.CreatedOn
                })
                .FirstOrDefault();

            if (data != null)
            {
                var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                data.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(data.CreatedOn, istZone);
            }

            return data;
        }

        public int SaveMediaItem(CreateUpdateMediaItemModel request)
        {
            MediaItem entity;

            if (request.Id > 0)
            {
                entity = _context.MediaItems.FirstOrDefault(x => x.Id == request.Id);

                if (entity == null)
                    return 0;
            }
            else
            {
                entity = new MediaItem
                {
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.MediaItems.Add(entity);
            }

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.MediaTypeId = request.MediaTypeId;
            entity.MediaUrl = request.MediaUrl;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Media");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            if (request.ThumbnailFile != null)
            {
                var uniqueFileName = Guid.NewGuid() + "_" + request.ThumbnailFile.FileName;

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.ThumbnailFile.CopyTo(stream);
                }

                entity.MediaThumnailPath = uniqueFileName;
            }

            _context.SaveChanges();

            return entity.Id;
        }
        public bool DeleteMediaItem(int id)
        {
            var entity = _context.MediaItems.FirstOrDefault(p => p.Id == id);

            if (entity == null)
                return false;

            entity.IsDeleted = true;

            _context.SaveChanges();

            return true;
        }

        public List<MediaItemTypeInfo> GetMediaItemTypes()
        {
            return _context.MediaItemTypes
                .Select(p => new MediaItemTypeInfo
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }
    }
}
