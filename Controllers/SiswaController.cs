using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace aegis_technical_tes.Controllers
{
    [Route("[controller]")]
    public class SiswaController : Controller
    {
        private readonly ILogger<SiswaController> _logger;

        public SiswaController(ILogger<SiswaController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("~/Views/Siswa/Index.cshtml");
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}