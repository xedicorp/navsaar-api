

using navsaar.api.Models;
using navsaar.api.ViewModels;
 

namespace navsaar.api.Repositories
{
    public interface IComplaintRepository
    {
        List<ComplaintInfo> List();
        ComplaintInfo GetById(int id);

        Task<bool> Save(CreateUpdateComplaintModel request);

        bool UpdateStatus(int id, int status);
        List<ComplaintType> GetComplaintTypes();

    }
}
