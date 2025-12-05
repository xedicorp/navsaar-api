
using Microsoft.EntityFrameworkCore;
using navsaar.api.Models;

namespace navsaar.api.Infrastructure
{
    public class AppDbContext :  DbContext // Ensure correct DbContext base class
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Township> Townships { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

    }
}
