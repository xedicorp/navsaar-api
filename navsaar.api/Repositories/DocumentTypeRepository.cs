

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
 

namespace navsaar.api.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly AppDbContext _context;
        public DocumentTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<DocumentType> List()
        {
            return _context.DocumentTypes.ToList();
                    

        }
    }
}
