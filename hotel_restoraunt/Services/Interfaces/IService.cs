using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}