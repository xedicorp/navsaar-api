using navsaar.api.ViewModels.Associate;

namespace navsaar.api.Repositories
{
    public interface IAssociateRepository
    {
        List<AssociateInfo> GetList();
        AssociateInfo GetById(long id);
        AssociateInfo GetByRera(string reraNo);

        Task<bool> Save(CreateUpdateAssociateModel model);
        bool Delete(long id);

    }
}
