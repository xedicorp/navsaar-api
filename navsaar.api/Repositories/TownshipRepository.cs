

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Inventory;
using OfficeOpenXml;
using System.Data;

namespace navsaar.api.Repositories
{
    public class TownshipRepository : ITownshipRepository
    {
        private readonly AppDbContext _context;
        IPlotRepository _plotRepository;
        public TownshipRepository(AppDbContext context, IPlotRepository plotRepository)
        {
            _context = context;
            _plotRepository = plotRepository;
        }
        public List<TownshipInfo> List(int userId = 0)
        {
            List<int> allowedTownshipIds = new List<int>();

             
            allowedTownshipIds= _context.UserTownships.Where(p => p.UserId == userId).Select(p=>p.TownshipId)            .ToList();

            return (from p in _context.Townships
                    where userId == 0 || allowedTownshipIds.Contains(p.Id)
                    select new TownshipInfo
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Address = p.Address
                        }).ToList();
           
        }

        public bool Save(TownshipCreateUpdateRequest request)
        {
            var entity = new Models.Township();
            entity.Name = request.Name;
            entity.Address = request.Address;
            if (request.Id>0)
            {
                entity = _context.Townships.Find(request.Id);
                if (entity == null)
                {
                    return false;
                }
                entity.Name  = request.Name;
                entity.Address=request.Address;
            }
            if (request.Id == 0)
                _context.Townships.Add(entity);

            _context.SaveChanges();
            return true;

        }

        public async Task<bool> UploadInventory(UploadInventoryRequestModel request)
        {
            string filePath = string.Empty;

            if (request.File != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }
            }

            // Read Excel
            DataTable dtblData = this.ExcelDataToDataTable(filePath, "Sheet1", true);

            var existingRecords = _context.Plots
                .Where(p => p.TownshipId == request.TownshipId)
                .ToList();

            var plotTypeList = _context.PlotTypes.ToList();
            var plotShapeList = _context.PlotShapes.ToList();

            foreach (DataRow row in dtblData.Rows)
            {
                string plotNo = row[0]?.ToString()?.Trim();
                decimal plotSize = Convert.ToDecimal(row[1]?.ToString() ?? "0");

                string plotShapeName = row[2]?.ToString()?.Trim();
                string facing = row[3]?.ToString()?.Trim();
                string plotType = row[4]?.ToString()?.Trim();
                string remark = row[5]?.ToString()?.Trim();
                string statusText = row[6]?.ToString()?.Trim();

                var existingPlot = existingRecords
                    .FirstOrDefault(p => p.PlotNo == plotNo);

                int plotId = existingPlot?.Id ?? 0;

                // Facing Mapping
                int facingId = facing?.ToUpper() switch
                {
                    "EAST" => 1,
                    "WEST" => 2,
                    "NORTH" => 3,
                    "SOUTH" => 4,
                    _ => 0
                };

                // Status Mapping
                int statusId = statusText?.ToUpper() switch
                {
                    "AVAILABLE" => 1,
                    "BOOKED" => 2,
                    "HOLD" => 3,
                    "RAHAN" => 5,
                    "NOT FOR SALE" => 9,
                    _ => 1
                };

                _plotRepository.Save(new CreateEditPlotRequest
                {
                    Id = plotId,
                    TownshipId = request.TownshipId,
                    PlotNo = plotNo,
                    PlotSize = plotSize,
                    Facing = facingId,
                    PlotTypeId = plotTypeList
                        .FirstOrDefault(p => p.Name.ToUpper() == plotType.ToUpper())?.Id ?? 0,

                    // ✅ New Fields
                    PlotShapeId = plotShapeList
                        .FirstOrDefault(s => s.ShapeName.ToUpper() == plotShapeName.ToUpper())?.Id,

                    Remark = remark,
                    Status = statusId
                });
            }

            return true;
        }
        public DataTable ExcelDataToDataTable(string filePath, string sheetName, bool hasHeader = true)
        {
            var dt = new DataTable();
            var fi = new FileInfo(filePath);

            if (!fi.Exists)
            {
                throw new Exception($"File {filePath} Does Not Exists");
            }
            ExcelPackage.License.SetNonCommercialOrganization("XEDI Corporation");
            // Set the license context for EPPlus (NonCommercial for free use)
            //  ExcelPackage.License.SetNonCommercialOrganization.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var xlPackage = new ExcelPackage(fi))
            {
                var worksheet = xlPackage.Workbook.Worksheets[sheetName];

                if (worksheet == null)
                {
                    throw new Exception($"Worksheet '{sheetName}' not found in the Excel file.");
                }

                // Determine the range of cells with data
                var startRow = hasHeader ? 2 : 1; // Start from row 2 if header exists, else row 1
                var endRow = worksheet.Dimension.End.Row;
                var startCol = 1;
                var endCol = worksheet.Dimension.End.Column;

                // Add columns to DataTable
                for (int col = startCol; col <= endCol; col++)
                {
                    string columnName = hasHeader ? worksheet.Cells[1, col].Text : $"Column{col}";
                    dt.Columns.Add(columnName);
                }

                // Add rows to DataTable
                for (int row = startRow; row <= endRow; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int col = startCol; col <= endCol; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }
        public bool Delete(int townshipId)
        {
            var township = _context.Townships.FirstOrDefault(x => x.Id == townshipId);
            if (township == null)
                return false;

            _context.Townships.Remove(township);
            _context.SaveChanges();

            return true;
        }

    }
}
