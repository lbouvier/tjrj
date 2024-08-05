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
    public class AutorController : Controller
    {
        private readonly BaseService<Autor> _autorService;

        public AutorController(BaseService<Autor> AutorService)
        {
            _autorService = AutorService;
        }

        // GET: Autor
        public async Task<IActionResult> Index()
        {
            var autores = (await _autorService.GetAllAsync()).OrderBy(x => x.CodAu);
            if (!string.IsNullOrEmpty(_autorService._repository.Mensagens))
            {
                TempData["error"] = _autorService._repository.Mensagens;
            }
            return View(autores);
        }

        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return View();
            }

            var Autor = await _autorService.GetByIdAsync(id);
            if (Autor == null)
            {
                TempData["error"] = _autorService._repository.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(Autor);
        }

        // GET: Autor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodAu,Nome")] Autor Autor)
        {
            if (ModelState.IsValid)
            {
                await _autorService.AddAsync(Autor);
                if (!string.IsNullOrEmpty(_autorService._repository.Mensagens))
                {
                    TempData["error"] = _autorService._repository.Mensagens;
                    return View(Autor);
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = "Cadastro realizado com sucesso!";
            return View(Autor);
        }

        // GET: Autor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var Autor = await _autorService.GetByIdAsync(id);
            if (Autor == null)
            {
                TempData["error"] = _autorService._repository.Mensagens;
                return View();
            }
            return View(Autor);
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodAu,Nome")] Autor Autor)
        {
            if (id != Autor.CodAu)
            {
                TempData["error"] = "Registro informado é diferente do registro à atualizar";
                return View();
            }

            if (ModelState.IsValid)
            {
                await _autorService.UpdateAsync(Autor);
                if (!string.IsNullOrEmpty(_autorService._repository.Mensagens))
                {
                    TempData["error"] = _autorService._repository.Mensagens;
                    return View(Autor);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Autor);
        }

        // GET: Autor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Autor = await _autorService.GetByIdAsync(id);
            if (Autor == null)
            {
                TempData["error"] = _autorService._repository.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(Autor);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _autorService.DeleteAsync(id);
            if (!string.IsNullOrEmpty(_autorService._repository.Mensagens))
            {
                TempData["error"] = _autorService._repository.Mensagens;
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _autorService.Any(e => e.CodAu == id);
        }
    }
}
