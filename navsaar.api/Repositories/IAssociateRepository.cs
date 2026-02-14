using navsaar.api.ViewModels.Associate;

namespace navsaar.api.Repositories
{
    public interface IAssociateRepository
    {
        List<AssociateInfo> GetList();
        AssociateInfo GetByRera(string reraNo);

        long Create(CreateUpdateAssociateModel model);
        bool Update(CreateUpdateAssociateModel model);
        bool Delete(long id);

    }
}
