
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Plot> Plots { get; set; }
        public DbSet<PlotType> PlotTypes { get; set; }

    }
}
