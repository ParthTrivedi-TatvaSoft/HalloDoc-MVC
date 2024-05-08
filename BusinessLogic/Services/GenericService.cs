using BusinessLogic.Interfaces;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {

        #region Constructor
        protected readonly ApplicationDbContext _dbContext;

        private readonly DbSet<T> _dbSet;

        public GenericService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        #endregion Constructor

        public virtual async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
            => await _dbSet.FirstOrDefaultAsync(filter);

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)
    ;
        }

        public async Task<T> GetByPropertyAsync(string property)
        {
            return await _dbSet.FindAsync(property);
        }

        public void Add(T model)
        {
            try
            {
                _dbContext.Add(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public void Update(T model)
        {
            try
            {
                _dbContext.Update(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            T model = _dbSet.FirstOrDefault(filter);
            return model;
        }
        public T IncludeEntity(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            T model = _dbSet.Include(select).FirstOrDefault(where);
            return model;
        }
        public List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null");
            }

            return _dbContext.Set<T>().Where(predicate).ToList();
        }

        public void UpdateAndSaveChanges(Expression<Func<T, bool>> predicate, Action<T> updateAction)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null");
            }

            var entitiesToUpdate = _dbContext.Set<T>().Where(predicate).ToList();
            entitiesToUpdate.ForEach(updateAction);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }

        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> model = _dbSet.Where(filter);
            return model;
        }
        public void OnlyUpdate(T model)
        {
            _dbContext.Update(model);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public int Count(Expression<Func<T, bool>> filter)
        {
            int count = _dbSet.Where(filter).Count();
            return count;
        }
        public void Remove(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
            _dbContext.SaveChanges();

        }
        public dynamic GetAllWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc)
        {
            if (IsAcc)
                return _dbSet.Where(where).OrderBy(orderBy).Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(select).ToList();

            return _dbSet.Where(where).OrderByDescending(orderBy).Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(select).ToList();
        }
        public int GetTotalcount(Expression<Func<T, bool>> where)
        {
            return _dbSet.Count(where);
        }
        public dynamic SelectWhere(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).Select(select).ToList();

        }
        public List<string> SelectWhere(Expression<Func<T, string>> select, Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).Select(select).ToList();

        }
        public dynamic SelectWhereOrderBy(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy)
        {
            return _dbSet.Where(where).OrderBy(orderBy).Select(select).ToList();

        }
    }
}