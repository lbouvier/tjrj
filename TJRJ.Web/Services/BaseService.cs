using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TJRJ.Web.Data;
using TJRJ.Entities;
using TJRJ.Repository;

namespace TJRJ.Services
{
    public class BaseService<T> : DbSet<T> where T : class
    {
        public readonly Repository<T> _repository;
        public string Mensagens;
        public BaseService(Repository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(T livro)
        {
            await _repository.AddAsync(livro);
        }

        public async Task UpdateAsync(T livro)
        {
            await _repository.UpdateAsync(livro);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
