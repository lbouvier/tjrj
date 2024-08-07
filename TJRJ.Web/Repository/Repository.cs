using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TJRJ.Web.Data;

namespace TJRJ.Repository
{
    public class Repository<T> where T : class
    {
        public readonly ApplicationDbContext _context;
        public readonly DbSet<T> _dbSet;
        public string Mensagens;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Mensagens = "Erro ao consultar os dados" + "\n" + ex.Message;
                return null;
            }
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            try
            {
                return (await _dbSet.FindAsync(id));
            }
            catch (Exception ex)
            {
                Mensagens = $"Erro ao consultar o registro com CÓDIGO {id}" + "\n" + ex.Message;
                return null;
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Mensagens = "Erro ao cadastrar o registro" + "\n" + ex.Message;
                return;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {

                _dbSet.Attach(entity);

                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                Mensagens = "Erro ao atualizar o registro" + "\n" + ex.Message;
                return;
            }
        }

        public async Task DeleteAsync(object id)
        {
            try
            {
                T entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {id} not found");
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                Mensagens = "Erro ao excluir o registro" + "\n" + ex.Message;
                return;
            }
        }
    }
}
