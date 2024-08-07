using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TJRJ.Web.Data;
using TJRJ.Entities;
using TJRJ.Repository;

namespace TJRJ.Services
{
    public class BaseService<T> : Repository<T> where T : class
    {
        public BaseService(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await GetAllAsync();
        }

        public async Task<T> GetById(int? id)
        {
            return await GetByIdAsync(id);
        }

        public async Task Create(T livro)
        {
            await AddAsync(livro);
        }

        public async Task Update(T livro)
        {
            await UpdateAsync(livro);
        }

        public async Task Delete(int id)
        {
            await DeleteAsync(id);
        }
    }
}
