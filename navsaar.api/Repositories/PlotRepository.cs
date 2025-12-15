using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;

namespace navsaar.api.Repositories
{
    public class PlotRepository : IPlotRepository
    {
        private readonly AppDbContext _context;
        public PlotRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<PlotInfo> List(int townshipId)
        {
            return (from p in _context.Plots
                    join t in _context.Townships on p.TownshipId equals t.Id
                    join pt in _context.PlotTypes on p.PlotTypeId equals pt.Id
                    where p.TownshipId == townshipId
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
                        FacingName = p.Facing == 1 ? "East": (p.Facing==2? "West": (p.Facing==3?"North":"South")),
                    }).ToList();

        }
        public  PlotInfo  GetById(int plotId)
        {
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
          
            if(request.Id == 0)
            {
                _context.Plots.Add(plot);
            }
            _context.SaveChanges();
            return plot.Id;

        }
    }
}
