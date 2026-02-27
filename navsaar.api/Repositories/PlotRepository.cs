using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.Services;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;

namespace navsaar.api.Repositories
{
    public class PlotRepository : IPlotRepository
    {
        private readonly AppDbContext _context;
        IWhatsAppService _whatsAppService;
        public PlotRepository(AppDbContext context, IWhatsAppService whatsAppService)
        {
            _context = context;
            _whatsAppService = whatsAppService;
        }
        public List<PlotInfo> List(int townshipId, int status = 0)
        {
            return (from p in _context.Plots
                    join t in _context.Townships on p.TownshipId equals t.Id
                    join pt in _context.PlotTypes on p.PlotTypeId equals pt.Id
                    join f in _context.FacingTypes on p.Facing equals f.Id
                    where p.TownshipId == townshipId
                    && (status == 0 || p.Status == status)
                    select new PlotInfo
                    {
                        Id = p.Id,
                        TownshipId = p.TownshipId,
                        Facing = p.Facing,
                        IsCorner = p.IsCorner,
                        IsTPoint = p.IsTPoint,
                        IsTapper = p.IsTapper,
                        PlotNo = p.PlotNo,
                        PlotSize = p.PlotSize,
                        PlotTypeId = p.PlotTypeId,
                        TownshipName = t.Name,
                        PlotTypeName = pt.Name,
                        FacingName = f.FacingName,
                        Status = GetStatus(p.Status ?? 0),
                        SaleableSize = p.SaleableSize,
                        PlotSizeInSqrmtr = p.PlotSizeInSqrmtr,
                        RoadSize = p.RoadSize,
                        PLC = p.PLC
                    }).ToList();

        }
        private static string GetStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return "Available";
                case 2:
                    return "Booked";
                case 3:
                    return "Hold";
                case 5:
                    return "Rahan";
                case 9:
                    return "Not for Sale";
                default:
                    return "Unknown";
            }
        }
        public  PlotInfo  GetById(int plotId)
        {
            ReleaseExpiredHolds();
            return (from p in _context.Plots
                    join t in _context.Townships on p.TownshipId equals t.Id
                    join pt in _context.PlotTypes on p.PlotTypeId equals pt.Id
                    where p.Id == plotId
                    select new PlotInfo
                    {
                        Id = p.Id,
                        TownshipId = p.TownshipId,
                        Facing = p.Facing,
                        IsCorner = p.IsCorner,
                        IsTPoint = p.IsTPoint,
                        IsTapper = p.IsTapper,
                        PlotNo = p.PlotNo,
                        PlotSize = p.PlotSize,
                        PlotTypeId = p.PlotTypeId,
                        TownshipName = t.Name,
                        PlotTypeName = pt.Name,
                        FacingName = p.Facing == 1 ? "East" : (p.Facing == 2 ? "West" : (p.Facing == 3 ? "North" : "South")),
                        SaleableSize = p.SaleableSize,
                        PlotSizeInSqrmtr = p.PlotSizeInSqrmtr,
                        RoadSize = p.RoadSize,
                        PLC = p.PLC,
                        Status = GetStatus(p.Status ?? 0)
                    }).FirstOrDefault();

        }
        public int Save(CreateEditPlotRequest request)
        {
            Plot plot = new Plot();
            if (request.Id>0)
            {
                plot = _context.Plots.Find(request.Id);
            }
            plot.PlotNo = request.PlotNo;
            plot.PlotSize = request.PlotSize;
            plot.TownshipId = request.TownshipId;
            plot.PlotTypeId = request.PlotTypeId;
            plot.Facing = request.Facing;   
            plot.IsCorner = request.IsCorner;   
            plot.IsTPoint = request.IsTPoint;   
            plot.IsTapper = request.IsTapper;
            plot.SaleableSize=request.SaleableSize;
            plot.PlotSizeInSqrmtr = request.PlotSizeInSqrmtr;
            plot.RoadSize = request.RoadSize;
            plot.PLC = request.PLC;


            if (request.Id == 0)
            {
                plot.Status = 1; // Available
                _context.Plots.Add(plot);

            }
            _context.SaveChanges();
            return plot.Id;

        }
        public bool HoldPlot(PlotHoldRequestModel model)
        {
            var associate = _context.Associates.FirstOrDefault(x => x.ID == model.AssociateId);
            var plot = _context.Plots.FirstOrDefault(x => x.Id == model.PlotId);

            if (plot == null || plot.Status != 1)
                return false;

            var existingHold = _context.PlotHoldRequests
                .Any(x => x.PlotId == model.PlotId && !x.IsDelete);

            if (existingHold)
                return false;

            plot.Status = 3; // Hold

            _context.PlotHoldRequests.Add(new PlotHoldRequest
            {
                PlotId = model.PlotId,
                AssociateId = model.AssociateId,
                HoldDateTime = model.HoldDateTime,
                IsDelete = false,
                WorkflowTypeId = model.WorkflowTypeId,
                TownshipId = model.TownshipId,
                PlotSize = model.PlotSize,
                AgreementRate = model.AgreementRate,
                TotalAgreementValue = model.TotalAgreementValue
            });

            _context.SaveChanges();

            _whatsAppService.SendMessage( BookingUpdate.Hold, plot.PlotNo, string.Format("{0} {1}", associate.FirstName , associate.LastName),
                 associate.ContactNo);
            return true;
        }

        public void ReleaseExpiredHolds()
        {
            var now = DateTime.Now;

            var expiredHolds = (from h in _context.PlotHoldRequests
                                join p in _context.Plots on h.PlotId equals p.Id
                                where !h.IsDelete
                                && h.HoldDateTime.AddHours(24) <= now
                                && p.Status == 3        // Still on hold
                                select new { h, p }).ToList();

            foreach (var item in expiredHolds)
            {
                item.p.Status = 1;   // Available
                item.h.IsDelete = true;
            }

            _context.SaveChanges();
        }


        public List<HoldPlotInfo> GetHoldPlots(int townshipId)
        {
            ReleaseExpiredHolds();
            var result = (from h in _context.PlotHoldRequests
                          join p in _context.Plots on h.PlotId equals p.Id
                          where !h.IsDelete
                                && p.Status == 3                 // HOLD
                                && p.TownshipId == townshipId    // FILTER
                          orderby h.HoldDateTime descending
                          select new HoldPlotInfo
                          {
                              PlotId = h.PlotId,
                              AssociateId = h.AssociateId,
                              HoldDateTime = h.HoldDateTime,

                              WorkflowTypeId = h.WorkflowTypeId ?? 0,
                              TownshipId = p.TownshipId,
                              PlotSize = h.PlotSize ?? 0,
                              AgreementRate = h.AgreementRate ?? 0,
                              TotalAgreementValue = h.TotalAgreementValue ?? 0,

                              Status = "Hold"
                          }).ToList();

            return result;
        }



    }
}
