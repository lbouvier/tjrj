using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TJRJ.Entities;
using TJRJ.Services;
using TJRJ.ViewModels;
using TJRJ.Web.Data;

namespace TJRJ.Controllers
{
    public class AutorController : Controller
    {
        private readonly BaseService<Autor> _autorService;
        private readonly IMapper _mapper;
        public AutorController(BaseService<Autor> AutorService, IMapper mapper)
        {
            _autorService = AutorService;
            _mapper = mapper;
        }

        // GET: Autor
        public async Task<IActionResult> Index()
        {
            var autores = (await _autorService.GetAll()).OrderByDescending(x => x.CodAu);
            if (!string.IsNullOrEmpty(_autorService.Mensagens))
            {
                TempData["error"] = _autorService.Mensagens;
            }
            return View(_mapper.Map<List<AutorViewModel>>(autores));
        }

        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return View();
            }

            var autor = await _autorService.GetById(id);
            if (autor == null)
            {
                TempData["error"] = _autorService.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<AutorViewModel>(autor));
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
        public async Task<IActionResult> Create([Bind("CodAu,Nome")] AutorViewModel autor)
        {
            if (ModelState.IsValid)
            {
                await _autorService.Create(_mapper.Map<Autor>(autor));
                if (!string.IsNullOrEmpty(_autorService.Mensagens))
                {
                    TempData["error"] = _autorService.Mensagens;
                    return View(autor);
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = "Cadastro realizado com sucesso!";
            return View(autor);
        }

        // GET: Autor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var autor = await _autorService.GetById(id);
            if (autor == null)
            {
                TempData["error"] = _autorService.Mensagens;
                return View();
            }
            return View(_mapper.Map<AutorViewModel>(autor));
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodAu,Nome")] AutorViewModel autor)
        {
            if (id != autor.CodAu)
            {
                TempData["error"] = "Registro informado é diferente do registro à atualizar";
                return View();
            }

            if (ModelState.IsValid)
            {
                await _autorService.Update(_mapper.Map<Autor>(autor));
                if (!string.IsNullOrEmpty(_autorService.Mensagens))
                {
                    TempData["error"] = _autorService.Mensagens;
                    return View(autor);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _autorService.GetById(id);
            if (autor == null)
            {
                TempData["error"] = _autorService.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<AutorViewModel>(autor));
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _autorService.Delete(id);
            if (!string.IsNullOrEmpty(_autorService.Mensagens))
            {
                TempData["error"] = _autorService.Mensagens;
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
