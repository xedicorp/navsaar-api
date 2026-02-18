using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels.Associate;
using ClosedXML.Excel;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssociateController : ControllerBase
    {
        private readonly IAssociateRepository _associateRepository;

        public AssociateController(IAssociateRepository associateRepository)
        {
            _associateRepository = associateRepository;
        }

        [HttpGet]
        [Route("GetList")]
        public List<AssociateInfo> GetList()
        {
            return _associateRepository.GetList();
        }

        [HttpGet]
        [Route("GetByRera")]
        public ActionResult<AssociateInfo> GetByRera(string reraNo)
        {
            var result = _associateRepository.GetByRera(reraNo);

            if (result == null)
                return NotFound("Associate not found for given RERA No");

            return Ok(result);
        }

        //Create
        [HttpPost("Save")]
        public async Task<ActionResult<bool>> Save([FromForm] CreateUpdateAssociateModel model)
        {
            var result = await _associateRepository.Save(model);

            if (!result)
                return NotFound("Associate not found");

            return Ok(true);
        }


        //DELETE
        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(long id)
        {
            var deleted = _associateRepository.Delete(id);

            if (!deleted)
                return NotFound("Associate not found");

            return Ok("Associate deleted successfully");
        }

        [HttpGet]
        [Route("ExportToExcel")]
        public IActionResult ExportToExcel()
        {
            var associates = _associateRepository.GetList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Associates");

            // ? Header
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 3).Value = "Name";
            worksheet.Cell(1, 4).Value = "Contact No";
            worksheet.Cell(1, 5).Value = "Leader Name";
            worksheet.Cell(1, 6).Value = "Leader Contact No";
            worksheet.Cell(1, 7).Value = "RERA No";

            // ? Data
            int row = 2;
            foreach (var a in associates)
            {
                worksheet.Cell(row, 1).Value = a.Id;
                worksheet.Cell(row, 3).Value = a.FirstName;
                worksheet.Cell(row, 4).Value = a.ContactNo;
                worksheet.Cell(row, 5).Value = a.LeaderName;
                worksheet.Cell(row, 6).Value = a.LeaderContactNo;
                worksheet.Cell(row, 7).Value = a.ReraNo;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Associate_List.xlsx"
            );
        }
    }
}
