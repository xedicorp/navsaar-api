using navsaar.api.ViewModels.Media;

namespace navsaar.api.Repositories
{
    public interface IMediaRepository
    {
        List<MediaItemInfo> GetMediaItems();
    }
}
