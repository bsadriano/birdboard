namespace Birdboard.API.Repositories.Interfaces;

public interface IRepository<T, U>
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T model);
    Task<T?> UpdateAsync(int id, U model);
}
