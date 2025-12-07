

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IReceiptRepository
    {
        List<ReceiptInfo> List();
       bool Save(CreateUpdateReceiptModel model);
    }
}
