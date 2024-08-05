using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TJRJ.Web.Entities;
using TJRJ.Web.Data;
using TJRJ.Web.Dtos;


namespace TJRJ.Web.Repository
{
    public class BaseRepository<T> : BaseRepositoryConsult<T> where T : EntidadeBase
    {
        protected readonly ApplicationDbContext _context;
        private DbSet<T> _dataSet;

        public BaseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dataSet = _context.Set<T>();
        }

        public async Task<bool> DeletetAsync(int codigo)
        {
            try
            {
                var result = await _dataSet.SingleOrDefaultAsync(x => x.Cod == codigo);
                if (result == null)
                {
                    return false;
                }
                _dataSet.Remove(result);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Messages.Add(new ReturnMessage
                {
                    Message = ex.Message,
                    Attribute = ex.InnerException.Message,
                    Property = ex.Source,
                    TranslatedMessage = "Not implemented"
                });
                return false;
            }
            return true;
        }

        public async Task<bool> ExistAsync(int codigo)
        {
            return await _dataSet.AnyAsync(x => x.Cod == codigo);
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                _dataSet.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Messages.Add(new ReturnMessage
                {
                    Message = ex.Message,
                    Attribute = ex.InnerException.Message,
                    Property = ex.Source,
                    TranslatedMessage = "Not implemented"
                });
                return null;
            }

            return entity;
        }

        public async Task<T> SelectAsync(int codigo)
        {
            try
            {
                return await _dataSet.SingleAsync(x => x.Cod == codigo);
            }
            catch (Exception ex)
            {
                Messages.Add(new ReturnMessage
                {
                    Message = ex.Message,
                    Attribute = ex.InnerException.Message,
                    Property = ex.Source,
                    TranslatedMessage = "Not implemented"
                });
                return null;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            try
            {
                return await _dataSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Messages.Add(new ReturnMessage
                {
                    Message = ex.Message,
                    Attribute = ex.InnerException.Message,
                    Property = ex.Source,
                    TranslatedMessage = "Not implemented"
                });
                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var result = await _dataSet.SingleOrDefaultAsync(x => x.Cod == entity.Cod);
                if (result == null)
                {
                    return null;
                }
                entity.UltimaAlteracao = DateTime.Now;
                _context.Entry(result).CurrentValues.SetValues(entity);
                _context.Entry(result).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Messages.Add(new ReturnMessage
                {
                    Message = ex.Message,
                    Attribute = ex.InnerException.Message,
                    Property = ex.Source,
                    TranslatedMessage = "Not implemented"
                });
                return null;
            }

            return entity;
        }
    }
}
