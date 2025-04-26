using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace aegis_technical_tes.Controllers
{
    [Route("api/[controller]")]
    public class FileDownloadController : Controller
    {
        private readonly ILogger<FileDownloadController> _logger;

        public FileDownloadController(ILogger<FileDownloadController> logger)
        {
            _logger = logger;
        }

        [HttpGet("download")]
        public IActionResult Download([FromQuery] string type)
        {
            if(type.ToLower() == "excel"){
                using var workbook = new XLWorkbook();
                var sheet = workbook.Worksheets.Add("Daftar Nama");

                List<string> names = new List<string> { "Junaedi", "Ali", "Dimas" };

                int row=1;
                foreach(var item in names){
                    sheet.Cell(row, 1).Value = @item;
                    row++;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                //set pointer kembali ke awal
                // stream.Position = 0;

                return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "contoh_excel.xlsx");
            }

            if(type.ToLower() == "pdf"){
                var pdf = new PdfDocument();
                var page = pdf.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);
                gfx.DrawString("Hello, M Farhan!", font, XBrushes.Black, new XPoint(100, 100));

                using (var stream = new MemoryStream())
                {
                    pdf.Save(stream);
                    return File(stream.ToArray(), "application/pdf", "contoh_pdf.pdf");
                }
                                
            };

            return BadRequest("Tipe File tidak tersedia");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}