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
    public class LivroController : Controller
    {
        private readonly AssuntoService _assuntoService;
        private readonly AutorService _autorService;
        private readonly LivroService _livroService;


        public LivroController(AssuntoService assuntoService, AutorService autorService, LivroService livroService)
        {
            _assuntoService = assuntoService;
            _autorService = autorService;
            _livroService = livroService;
        }

        // GET: Livro
        public async Task<IActionResult> Index()
        {
            var livros = (await _livroService.GetAllAsync()).OrderByDescending(x => x.Cod);
            if (!string.IsNullOrEmpty(_livroService._repository.Mensagens))
            {
                TempData["error"] = _livroService._repository.Mensagens;
            }
            return View(livros);
        }

        // GET: Livro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var livro = await _livroService.GetByIdAsync(id);
            if (livro == null)
            {
                TempData["error"] = _livroService._repository.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(livro);
        }

        // GET: Livro/Create
        public async Task<IActionResult> Create()
        {
            var autores = (await _autorService.GetAllAsync()).OrderBy(x => x.CodAu);
            var assuntos = (await _assuntoService.GetAllAsync()).OrderBy(x => x.CodAs);
            ViewBag.LivroAutores = new SelectList(autores, "CodAu", "Nome");
            ViewBag.LivroAssuntos = new SelectList(assuntos, "CodAs", "Descricao");
            return View();
        }

        // POST: Livro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cod,Titulo,Editora,Edicao,AnoPublicacao,LivroAutores,LivroAssuntos,Preco")] Livro livro, int[] LivroAutores, int[] LivroAssuntos)
        {
            if (ModelState.IsValid)
            {
                if (LivroAutores != null)
                {
                    foreach (var livroAutorCodigo in LivroAutores)
                    {
                        var autor = await _autorService.GetByIdAsync(livroAutorCodigo);
                        if (autor != null)
                        {
                            livro.LivroAutores.Add(new LivroAutor { Autor = autor });
                        }
                    }
                }

                if (LivroAssuntos != null)
                {
                    foreach (var assuntoCodigo in LivroAssuntos)
                    {
                        var assunto = await _assuntoService.GetByIdAsync(assuntoCodigo);
                        if (assunto != null)
                        {
                            livro.LivroAssuntos.Add(new LivroAssunto { Assunto = assunto });
                        }
                    }
                }
                await _livroService.AddAsync(livro);
                if (!string.IsNullOrEmpty(_livroService._repository.Mensagens))
                {
                    TempData["error"] = _livroService._repository.Mensagens;
                    return View(livro);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livro);
        }

        // GET: Livro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService.GetByIdIncludes(id.Value);

            if (livro == null)
            {
                TempData["error"] = _livroService._repository.Mensagens;
                return View();
            }

            ViewBag.LivroAssuntos = (await _assuntoService.GetAllAsync()).OrderBy(x => x.CodAs).ToList();
            ViewBag.LivroAutores = (await _autorService.GetAllAsync()).OrderBy(x => x.CodAu).ToList();
            return View(livro);
        }

        // POST: Livro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cod,Titulo,Editora,Edicao,AnoPublicacao,LivroAutores,LivroAssuntos,Preco")] Livro livro, int[] autores, int[] assuntos)
        {
            if (id != livro.Cod)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (autores != null)
                {
                    foreach (var livroAutorCodigo in autores)
                    {
                        livro.LivroAutores.Add(new LivroAutor { AutorCodAu = livroAutorCodigo, LivroCod = id });
                        //var autor = await _autorService.GetByIdIncludes(livroAutorCodigo);
                        //if (autor != null)
                        //{
                        //    //_autorService._repository._context.Entry(livro).State = EntityState.Detached;
                            
                        //}
                    }
                }

                if (assuntos != null)
                {
                    foreach (var assuntoCodigo in assuntos)
                    {
                        livro.LivroAssuntos.Add(new LivroAssunto { AssuntoCodAs = assuntoCodigo });
                        //var assunto = await _assuntoService.GetByIdIncludes(assuntoCodigo);
                        //if (assunto != null)
                        //{
                           
                        //}
                    }
                }

                await _livroService.UpdateAsync(id, livro);
                if (!string.IsNullOrEmpty(_livroService._repository.Mensagens))
                {
                    TempData["error"] = _livroService._repository.Mensagens;
                    ViewBag.LivroAssuntos = (await _assuntoService.GetAllAsync()).OrderBy(x => x.CodAs).ToList();
                    ViewBag.LivroAutores = (await _autorService.GetAllAsync()).OrderBy(x => x.CodAu).ToList();
                    return View(livro);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livro);
        }

        // GET: Livro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService.GetByIdAsync(id);
            if (livro == null)
            {
                return View();
            }

            return View(livro);
        }

        // POST: Livro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _livroService.DeleteAsync(id);
            if (!string.IsNullOrEmpty(_livroService._repository.Mensagens))
            {
                TempData["error"] = _livroService._repository.Mensagens;
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _livroService.Any(e => e.Cod == id);
        }
    }
}
