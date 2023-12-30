using Register_Supply_Management.Contracts;
using Register_Supply_Management.Data;

namespace Register_Supply_Management.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
    {
        protected readonly RegisterDBContext _registerDbContext;

        public GeneralRepository(RegisterDBContext registerDbContext)
        {
            _registerDbContext = registerDbContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _registerDbContext.Set<TEntity>().ToList();
        }

        public TEntity? GetById(int id)
        {
            var entity = _registerDbContext.Set<TEntity>().Find(id);
            _registerDbContext.ChangeTracker.Clear();
            return entity;
        }

        public TEntity? Create(TEntity entity)
        {
            try
            {
                _registerDbContext.Set<TEntity>().Add(entity);
                _registerDbContext.SaveChanges();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(TEntity entity)
        {
            try
            {
                _registerDbContext.Set<TEntity>().Update(entity);
                _registerDbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(TEntity entity)
        {
            try
            {
                _registerDbContext.Set<TEntity>().Remove(entity);
                _registerDbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsExits(int id)
        {
            return GetById(id) != null;
        }

    }
}
