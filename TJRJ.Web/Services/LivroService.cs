using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using TJRJ.Entities;
using TJRJ.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TJRJ.Services
{
    public class LivroService : BaseService<Livro>
    {
        public readonly Repository<Livro> _repository;
        //private readonly DbSet<Livro> _dbSet;

        public LivroService(Repository<Livro> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Livro> GetByIdIncludes(long? id)
        {
            IQueryable<Livro> query = _repository._context.Livros.AsNoTracking();
            var livro = await query.Include(x => x.LivroAutores)
                                             .ThenInclude(la => la.Autor)
                                             .Include(x => x.LivroAssuntos).ThenInclude(x => x.Assunto)
                                             .FirstOrDefaultAsync(x => x.Cod == id.Value);



            return livro;
        }

        public async Task<Livro> UpdateAsync(int? id, Livro livro)
        {
            try
            {
                _repository._context.Entry(livro).State = EntityState.Modified;
                await _repository.UpdateAsync(livro);
            }
            catch (Exception ex)
            {
                _repository.Mensagens = ex.Message;
            }
            return livro;
        }
    }
}
