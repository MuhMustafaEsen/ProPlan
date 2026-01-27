using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Abstract
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        //yeni ekledim
        Task<T?> GetByIdAsync(int id, bool trackChanges);
        //yeni ekledim
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);

        void Create(T entity);
        //yeni ekledim
        void CreateRange(IEnumerable<T> entities);
        void Update(T entity);  
        void Delete(T entity);

    }
}
