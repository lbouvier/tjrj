using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using TJRJ.Entities;
using TJRJ.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TJRJ.Services
{
    public class AssuntoService : BaseService<Assunto>
    {
        private readonly Repository<Assunto> _repository;
        private readonly DbSet<Assunto> _dbSet;

        public AssuntoService(Repository<Assunto> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Assunto> GetByIdIncludes(long? id, params Expression<Func<Assunto, object>>[] includes)
        {
            IQueryable<Assunto> query = _repository._dbSet;
            var assunto = await query.Include(x => x.LivroAssuntos)
                                             .ThenInclude(la => la.Assunto)
                                             .FirstOrDefaultAsync(x => x.CodAs == id.Value);

            return assunto;
        }
    }
}
