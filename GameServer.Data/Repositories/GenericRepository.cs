using System.Linq.Expressions;

namespace GameServer.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly GameServerDbContext _context;
        public GenericRepository(GameServerDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void AddRange(ICollection<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(ICollection<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}