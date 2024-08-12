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
using TJRJ.ViewModels;
using FastReport.Export.PdfSimple;
using System.Collections;

namespace TJRJ.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly BaseService<RelatorioViewModel> _livroService;
        public class Teste
        {
            public int Id { get; set; }
            public string Descricao { get; set; }
        }
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnv, BaseService<RelatorioViewModel> livroService)
        {
            _logger = logger;
            _webHostEnv = webHostEnv;
            _livroService = livroService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Relatorio")]
        public IActionResult Relatorio()
        {
            using (var report = new Report())
            {
                try
                {
                    var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports\ReportMvc.frx");
                    var reportFile = caminhoReport;
                    // Carrega o arquivo .frx do relatório
                    var livros = _livroService._context.RelatorioViewModel.ToList();
                    report.Load(caminhoReport);
                    report.RegisterData(_livroService._context.RelatorioViewModel.ToList(), "livros");
                    report.GetDataSource("livros");
                    report.Dictionary.RegisterBusinessObject(livros, "livros", 10, true);
                    // Prepara o relatório
                    report.Prepare();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao preparar o relatório: " + ex.Message);
                    return BadRequest("Erro ao preparar o relatório.");
                }

                using (var ms = new MemoryStream())
                {
                    var pdfExport = new PDFSimpleExport();

                    try
                    {
                        // Exporta o relatório para o MemoryStream
                        report.Export(pdfExport, ms);

                        // Certifique-se de que o MemoryStream não está vazio
                        if (ms.Length == 0)
                        {
                            throw new Exception("O PDF gerado está vazio.");
                        }

                        ms.Position = 0;

                        // Retorna o PDF como um arquivo para o navegador
                        return File(ms.ToArray(), "application/pdf");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Erro ao exportar para PDF." + ex.Message);
                    }
                }
            }
        }

        [HttpGet("CreateReport")]
        public async Task<IActionResult> CreateReport()
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports\ReportMvc.frx");
            var reportFile = caminhoReport;
            //if (System.IO.File.Exists(caminhoReport))
            //{
            //    System.IO.File.Delete(caminhoReport);
            //}

            var freport = new FastReport.Report();
            var livros = await _livroService.GetAll();
            
            freport.Dictionary.RegisterBusinessObject(livros, "livros", 10, true);
            freport.Report.Save(reportFile);
            return RedirectToAction("Index");
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
