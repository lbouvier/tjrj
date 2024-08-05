using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using TJRJ.Web.Entities;
using TJRJ.Web.Data;
using TJRJ.Web.Dtos;

namespace TJRJ.Web.Repository
{
    public class BaseRepositoryConsult<T> where T : EntidadeBase
    {
        protected readonly ApplicationDbContext _context;
        private DbSet<T> _dataSet;
        public IList<ReturnMessage> Messages { get; } = new List<ReturnMessage>();
        public BaseRepositoryConsult(ApplicationDbContext context)
        {
            _context = context;
            _dataSet = _context.Set<T>();
        }

        public List<T> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "")
        {
            IQueryable<T> query = _dataSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderby != null)
            {
                query = orderby(query);
            }

            // If you use the First() method you will get an exception when the result is empty.
            return query.ToList();
        }
    }
}
