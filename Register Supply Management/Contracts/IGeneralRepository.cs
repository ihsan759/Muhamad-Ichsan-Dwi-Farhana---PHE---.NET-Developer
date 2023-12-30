namespace Register_Supply_Management.Contracts
{
    public interface IGeneralRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity? GetById(int id);
        TEntity? Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        bool IsExits(int id);
    }
}
