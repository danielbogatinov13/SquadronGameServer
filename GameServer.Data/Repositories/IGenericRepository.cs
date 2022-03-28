using System.Linq.Expressions;

namespace GameServer.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        void Remove(T entity);
        void RemoveRange(ICollection<T> entities);
    }
}