using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models;
using OutOfOffice.Services.Data;
using System.Linq.Expressions;

namespace OutOfOffice.Services.Repository.EntityFramework
{
    public class RepositoryService<T> : IRepositoryService<T> where T : class, IEntity<int>
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _set;

        public RepositoryService(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public virtual ServiceResult Add(T entity)
        {
            ServiceResult result = new();
            try
            {
                _ = _set.Add(entity);
                result = Save();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }

            return result;
        }
        public virtual ServiceResult Delete(T entity)
        {
            ServiceResult result = new();
            try
            {
                _ = _set.Remove(entity);
                result = Save();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;
        }

        public virtual ServiceResult Edit(T entity)
        {
            ServiceResult result = new();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;

                result = Save();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;
        }


        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _set.Where(predicate);
            return query;
        }
        public IQueryable<T> GetAllRecords()
        {
            return _set;
        }
        public virtual T? GetSingle(int id)
        {

            T? result = _set.FirstOrDefault(r => r.Id == id);

            return result ?? null;
        }
        public virtual ServiceResult Save()
        {
            ServiceResult result = new();
            try
            {
                _ = _context.SaveChanges();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;

        }
    }

}
