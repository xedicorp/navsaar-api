
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
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Plot> Plots { get; set; }
        public DbSet<PlotType> PlotTypes { get; set; }
        public DbSet<Followup> Followups { get; set; }
        public DbSet<FollowupType> FollowupTypes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<UserTownship> UserTownships { get; set; }
        public DbSet<PlotChangeLog> PlotChangeLogs { get; set; }
        public DbSet<BookingCancelLog> BookingCancelLogs { get; set; }
        public DbSet<RefundRequest> RefundRequests { get; set; }
        public DbSet<RefundStatusLog> RefundStatusLogs { get; set; }
        public DbSet<AssociateInfo> Associates { get; set; }
        public DbSet<ReceiptVerificationRequest> ReceiptVerificationRequests { get; set; }
        public DbSet<DraftRequest> DraftRequests { get; set; }
        public DbSet<AllotmentLetterRequest> AllotmentLetterRequests { get; set; }
        public DbSet<FacingType> FacingTypes { get; set; }
        public DbSet<WorkflowDocType> WorkflowDocTypes { get; set; }
        public DbSet<BookingStatusType> BookingStatusTypes { get; set; }
        public DbSet<FileTimeline> FileTimelines { get; set; }
        public DbSet<AppSetting> AppSettings{ get; set;}
        public DbSet<PlotHoldRequest> PlotHoldRequests { get; set; }


    }
}
