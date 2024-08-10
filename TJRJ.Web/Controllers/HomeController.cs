using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TJRJ.Entities;
using TJRJ.Services;
using TJRJ.Web.Models;
using FastReport;

namespace TJRJ.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly BaseService<Livro> _livroService;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnv, BaseService<Livro> livroService)
        {
            _logger = logger;
            _webHostEnv = webHostEnv;
            _livroService = livroService;
        }

        public IActionResult Index()
        {
          
            return View();
        }

        [HttpGet("CreateReport")]
        public async Task<IActionResult> CreateReport()
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports\ReportMvc.frx");
            var reportFile = caminhoReport;
            var freport = new FastReport.Report();
            var livros = await _livroService.GetAll();

            freport.Dictionary.RegisterBusinessObject(livros, "livros", 10, true);
            freport.Report.Save(reportFile);
            return View($"Relatório Gerado: {caminhoReport}" );
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
