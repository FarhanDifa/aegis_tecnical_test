using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using aegis_technical_tes.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace aegis_technical_tes.Controllers
{
    [Route("[controller]")]
    public class SiswaController : Controller
    {
        private readonly ILogger<SiswaController> _logger;
        private readonly ISiswaRepository _siswaRepository;


        public SiswaController(ILogger<SiswaController> logger, ISiswaRepository siswaRepository)
        {
            _logger = logger;
            _siswaRepository = siswaRepository;
        }

        public IActionResult Index()
        {
            var isExportingPdf = false; 
            var result = _siswaRepository.GetAll();

            ViewBag.IsExportingPdf = isExportingPdf;
            return View("~/Views/Siswa/Index.cshtml",result);
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}