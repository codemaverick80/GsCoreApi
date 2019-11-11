using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GsCore.Database.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
       
       IQueryable<T> FindAll();
       IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
                
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

       // void Update(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        int Count(Func<T, bool> predicate);        
    }

 }
