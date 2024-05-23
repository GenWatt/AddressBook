using AddressBook.Models;

namespace AddressBook.Repository;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetById(int id);
    Task<TEntity?> GetById(TEntity entity);
    Task<IEnumerable<TEntity>> GetAll();

    void Insert(TEntity entity);
    void Update(TEntity entity);

    void Delete(int id);
    void Delete(TEntity entity);
}