using navsaar.api.Models;
using navsaar.api.ViewModels.Media;

namespace navsaar.api.Repositories
{
    public interface IMediaRepository
    {
        List<MediaItemInfo> GetMediaItems(int? mediaTypeId = null, int ? townshipId = null);
        List<MediaItemTypeInfo> GetMediaItemTypes();
        MediaItemInfo? GetMediaItemById(int id);
        int SaveMediaItem(CreateUpdateMediaItemModel model);
        bool DeleteMediaItem(int id);
    }
}
