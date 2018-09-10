using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Test.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Find(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int pageIndex = 1, int pageSize = 10);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
