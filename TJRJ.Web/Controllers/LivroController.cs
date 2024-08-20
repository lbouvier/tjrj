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
    public class LivroController : Controller
    {
        private readonly BaseService<Assunto> _assuntoService;
        private readonly BaseService<Autor> _autorService;
        private readonly BaseService<Livro> _livroService;
        private readonly BaseService<LivroAutor> _livroAutorService;
        private readonly BaseService<LivroAssunto> _livroAssuntoService;
        private readonly IMapper _mapper;


        public LivroController(BaseService<Assunto> assuntoService, BaseService<Autor> autorService, BaseService<Livro> livroService, IMapper mapper, BaseService<LivroAutor> livroAutorService, BaseService<LivroAssunto> livroAssuntoService)
        {
            _assuntoService = assuntoService;
            _autorService = autorService;
            _livroService = livroService;
            _mapper = mapper;
            _livroAutorService = livroAutorService;
            _livroAssuntoService = livroAssuntoService;
        }

        // GET: Livro
        public async Task<IActionResult> Index()
        {
            var livros = (await _livroService.GetAll()).OrderByDescending(x => x.Cod);

            if (!string.IsNullOrEmpty(_livroService.Mensagens))
            {
                TempData["error"] = _livroService.Mensagens;
            }
            return View(_mapper.Map<List<LivroViewModel>>(livros));
        }

        // GET: Livro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var livro = await _livroService.GetById(id);
            if (livro == null)
            {
                TempData["error"] = _livroService.Mensagens;
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<LivroViewModel>(livro));
        }

        // GET: Livro/Create
        public async Task<IActionResult> Create()
        {
            var autores = (await _autorService.GetAll()).OrderBy(x => x.CodAu);
            var assuntos = (await _assuntoService.GetAll()).OrderBy(x => x.CodAs);
            ViewBag.LivroAutores = new SelectList(autores, "CodAu", "Nome");
            ViewBag.LivroAssuntos = new SelectList(assuntos, "CodAs", "Descricao");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cod,Titulo,Editora,Edicao,AnoPublicacao,LivroAutores,LivroAssuntos,Preco")] LivroViewModel livroView, int[] LivroAutores, int[] LivroAssuntos)
        {
            if (ModelState.IsValid)
            {
                if (LivroAutores != null)
                {
                    foreach (var livroAutorCodigo in LivroAutores)
                    {
                       livroView.LivroAutores.Add(new LivroAutorViewModel { AutorCodAu = livroAutorCodigo, LivroCod = livroView .Cod});
                    }
                }

                if (LivroAssuntos != null)
                {
                    foreach (var assuntoCodigo in LivroAssuntos)
                    {
                        livroView.LivroAssuntos.Add(new LivroAssuntoViewModel { AssuntoCodAs = assuntoCodigo, LivroCod = livroView.Cod });
                    }
                }

                await _livroService.Create(_mapper.Map<Livro>(livroView));
                if (!string.IsNullOrEmpty(_livroService.Mensagens))
                {
                    TempData["error"] = _livroService.Mensagens;
                    return View(livroView);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var autores = (await _autorService.GetAll()).OrderBy(x => x.CodAu);
                var assuntos = (await _assuntoService.GetAll()).OrderBy(x => x.CodAs);
                ViewBag.LivroAutores = new SelectList(autores, "CodAu", "Nome");
                ViewBag.LivroAssuntos = new SelectList(assuntos, "CodAs", "Descricao");
            }
            return View(livroView);
        }

        // GET: Livro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService._dbSet.Include(x => x.LivroAutores)
                                                  .ThenInclude(la => la.Autor)
                                                  .Include(x => x.LivroAssuntos)
                                                  .ThenInclude(x => x.Assunto)
                                                  .FirstOrDefaultAsync(x => x.Cod == id.Value);

            if (livro == null)
            {
                TempData["error"] = _livroService.Mensagens;
                return View();
            }

            ViewBag.LivroAssuntos = (await _assuntoService.GetAll()).OrderBy(x => x.CodAs).ToList();
            ViewBag.LivroAutores = (await _autorService.GetAll()).OrderBy(x => x.CodAu).ToList();
            return View(_mapper.Map<LivroViewModel>(livro));
        }

        // POST: Livro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cod,Titulo,Editora,Edicao,AnoPublicacao,LivroAutores,LivroAssuntos,Preco")] LivroViewModel livroView, int[] autores, int[] assuntos)
        {

            if (id != livroView.Cod)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                _livroAutorService._context.LivroAutores.RemoveRange(_livroAutorService._context.LivroAutores.Where(x => x.LivroCod == id));
                _livroAssuntoService._context.LivroAssuntos.RemoveRange(_livroAssuntoService._context.LivroAssuntos.Where(x => x.LivroCod == id));
                foreach (var livroAutorCodigo in autores)
                {
                    var autor = await _autorService.GetById(livroAutorCodigo);
                    if (autor != null)
                    {
                        _autorService._context.Entry(autor).State = EntityState.Detached;
                        livroView.LivroAutores.Add(new LivroAutorViewModel { AutorCodAu = livroAutorCodigo, LivroCod = id });
                    }
                }

                if (assuntos != null)
                {
                    foreach (var assuntoCodigo in assuntos)
                    {
                        var assunto = await _assuntoService.GetById(assuntoCodigo);
                        if (assunto != null)
                        {
                            _assuntoService._context.Entry(assunto).State = EntityState.Detached;
                            livroView.LivroAssuntos.Add(new LivroAssuntoViewModel { AssuntoCodAs = assuntoCodigo, LivroCod = id });
                        }
                    }
                }
            
                await _livroService.Update(_mapper.Map<Livro>(livroView));

                if (!string.IsNullOrEmpty(_livroService.Mensagens))
                {
                    TempData["error"] = _livroService.Mensagens;
                    ViewBag.LivroAssuntos = (await _assuntoService.GetAll()).OrderBy(x => x.CodAs).ToList();
                    ViewBag.LivroAutores = (await _autorService.GetAll()).OrderBy(x => x.CodAu).ToList();
                    return View(livroView);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livroView);
        }

        // GET: Livro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _livroService.GetById(id);
            if (livro == null)
            {
                return View();
            }

            return View(_mapper.Map<LivroViewModel>(livro));
        }

        // POST: Livro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _livroService.Delete(id);
            if (!string.IsNullOrEmpty(_livroService.Mensagens))
            {
                TempData["error"] = _livroService.Mensagens;
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
