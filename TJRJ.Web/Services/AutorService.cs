using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using TJRJ.Entities;
using TJRJ.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TJRJ.Services
{
    public class AutorService : BaseService<Autor>
    {
        private readonly Repository<Autor> _repository;
        private readonly DbSet<Autor> _dbSet;

        public AutorService(Repository<Autor> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Autor> GetByIdIncludes(long? id)
        {
            IQueryable<Autor> query = _repository._dbSet;
            var autor = await query.Include(x => x.LivroAutores)
                                             .ThenInclude(la => la.Autor)
                                             .FirstOrDefaultAsync(x => x.CodAu == id.Value);

            return autor;
        }
    }
}
