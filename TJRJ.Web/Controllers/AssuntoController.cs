using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TJRJ.Entities;
using TJRJ.Services;
using TJRJ.Web.Data;

namespace TJRJ.Controllers
{
    public class AssuntoController : Controller
    {
        private readonly BaseService<Assunto> _assuntoService;

        public AssuntoController(BaseService<Assunto> assuntoService)
        {
            _assuntoService = assuntoService;
        }

        // GET: Assunto
        public async Task<IActionResult> Index()
        {
            var assuntos = (await _assuntoService.GetAllAsync()).OrderBy(x => x.CodAs);
            if (!string.IsNullOrEmpty(_assuntoService._repository.Mensagens))
            {
                TempData["error"] = _assuntoService._repository.Mensagens;
            }
            return View(assuntos);
        }

        // GET: Assunto/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return View();
            }

            var assunto = await _assuntoService.GetByIdAsync(id);
            if (assunto == null)
            {
                TempData["error"] = _assuntoService._repository.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(assunto);
        }

        // GET: Assunto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assunto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodAs,Descricao")] Assunto assunto)
        {
            if (ModelState.IsValid)
            {
                await _assuntoService.AddAsync(assunto);
                if (!string.IsNullOrEmpty(_assuntoService._repository.Mensagens))
                {
                    TempData["error"] = _assuntoService._repository.Mensagens;
                    return View(assunto);
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = "Cadastro realizado com sucesso!";
            return View(assunto);
        }

        // GET: Assunto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var assunto = await _assuntoService.GetByIdAsync(id);
            if (assunto == null)
            {
                TempData["error"] = _assuntoService._repository.Mensagens;
                return View();
            }
            return View(assunto);
        }

        // POST: Assunto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodAs,Descricao")] Assunto assunto)
        {
            if (id != assunto.CodAs)
            {
                TempData["error"] = "Registro informado é diferente do registro à atualizar";
                return View();
            }

            if (ModelState.IsValid)
            {
                await _assuntoService.UpdateAsync(assunto);
                if (!string.IsNullOrEmpty(_assuntoService._repository.Mensagens))
                {
                    TempData["error"] = _assuntoService._repository.Mensagens;
                    return View(assunto);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(assunto);
        }

        // GET: Assunto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assunto = await _assuntoService.GetByIdAsync(id);
            if (assunto == null)
            {
                TempData["error"] = _assuntoService._repository.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(assunto);
        }

        // POST: Assunto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _assuntoService.DeleteAsync(id);
            if (!string.IsNullOrEmpty(_assuntoService._repository.Mensagens))
            {
                TempData["error"] = _assuntoService._repository.Mensagens;
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AssuntoExists(int id)
        {
            return _assuntoService.Any(e => e.CodAs == id);
        }
    }
}
