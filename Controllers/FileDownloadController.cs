using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using aegis_technical_tes.Repositories.Contracts;
using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace aegis_technical_tes.Controllers
{
    [Route("[controller]")]
    public class FileDownloadController : Controller
    {
        private readonly ILogger<FileDownloadController> _logger;
        private readonly ISiswaRepository _siswaRepository;
        private readonly IConverter _converter;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public FileDownloadController(
            ILogger<FileDownloadController> logger, 
            ISiswaRepository siswaRepository,
            IConverter converter,
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider)
        {
            _logger = logger;
            _siswaRepository = siswaRepository;
            _converter = converter;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }
        
        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string type)
        {
            if(type.ToLower() == "excel"){
                using var workbook = new XLWorkbook();
                var sheet = workbook.Worksheets.Add("Daftar Siswa");

                var listSiswa = _siswaRepository.GetAll();

                int row=1;
                foreach(var item in listSiswa){
                    sheet.Cell(row, 1).Value = item.Nama;
                    sheet.Cell(row, 2).Value = item.Umur.ToString();
                    sheet.Cell(row, 3).Value = item.Alamat;
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
                // var pdf = new PdfDocument();
                // var page = pdf.AddPage();
                // var gfx = XGraphics.FromPdfPage(page);
                // var font = new XFont("Arial", 12);
                // gfx.DrawString("Hello, M Farhan!", font, XBrushes.Black, new XPoint(100, 100));

                // using (var stream = new MemoryStream())
                // {
                //     pdf.Save(stream);
                //     return File(stream.ToArray(), "application/pdf", "contoh_pdf.pdf");
                // }
                var model = _siswaRepository.GetAll();

                var htmlContent = await RenderViewToStringAsync("~/Views/Siswa/Index.cshtml", model);

                await new BrowserFetcher().DownloadAsync();

                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                using var page = await browser.NewPageAsync();

                await page.SetContentAsync(htmlContent);

                var pdf = await page.PdfDataAsync(new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true
                });

                return File(pdf, "application/pdf", "contoh_pdf.pdf");
            };

            return BadRequest("Tipe File tidak tersedia");

        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);
            
            var viewResult = _razorViewEngine.GetView(null, "~/Views/Siswa/Index.cshtml", false);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{viewName} not found.");
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            await using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                new TempDataDictionary(HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }


        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
    
}