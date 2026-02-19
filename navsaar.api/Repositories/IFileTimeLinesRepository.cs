using navsaar.api.ViewModels.FileTimelines;

namespace navsaar.api.Repositories
{
    public interface IFileTimelinesRepository
    {
        List<FileTimelinesInfo> GetList();
        FileTimelinesInfo GetById(int id);
        Task<bool> Save(CreateUpdateFileTimelinesModel model);
        bool Delete(int id);
    }
}
