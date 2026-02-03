

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
                // Define the path where the file will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string fileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                  filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(stream);
                }
            }

            //Read excel and save data
            DataTable dtblData = this.ExcelDataToDataTable(filePath, "Sheet1", true);

           var existingRecords=   _context.Plots.Where(p => p.TownshipId == request.TownshipId ).ToList();

            foreach (DataRow row in dtblData.Rows)
            {
                string plotNo = row[0].ToString();
                decimal plotSize = Convert.ToDecimal(row[1].ToString());
                string isCorner = row[2].ToString();
                string isTapper = row[3].ToString();
                string isTPoint = row[4].ToString();
                string facing = row[5].ToString();
                string plotType = row[6].ToString();
                var existingPlot = existingRecords.Where(p => p.TownshipId == request.TownshipId && p.PlotNo == plotNo).ToList();
                var plotId = existingPlot == null ? 0 : existingPlot.FirstOrDefault().Id;
                var plotTypeList = _context.PlotTypes.ToList();
                
                int facingId = 0;
                if (facing.ToUpper() == "EAST")
                    facingId = 1;
                else if (facing.ToUpper() == "WEST")
                    facingId = 2;
                else if (facing.ToUpper() == "NORTH")
                    facingId = 3;
                else if (facing.ToUpper() == "SOUTH")
                    facingId = 4;

                _plotRepository.Save(new CreateEditPlotRequest
                {
                    Id = plotId,
                    TownshipId = request.TownshipId,
                    PlotNo = plotNo,
                    PlotSize = plotSize,
                    Facing = facingId,
                    IsCorner = isCorner.ToUpper() == "YES" ? true : false,
                    IsTPoint = isTPoint.ToUpper() == "YES" ? true : false,
                    IsTapper = isTapper.ToUpper() == "YES" ? true : false,
                    PlotTypeId = plotTypeList.FirstOrDefault(p => p.Name == plotType)?.Id ?? 0,
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

    }
}
