using AddressBook.Data;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Repository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Delete(int id)
    {
        _context.Remove(id);
    }

    public void Delete(TEntity entity)
    {
        _context.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetById(int id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<TEntity?> GetById(TEntity entity)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == entity.Id);
    }

    public void Insert(TEntity entity)
    {
        entity.CreatedAt = DateTime.Now;
        _context.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }
}

