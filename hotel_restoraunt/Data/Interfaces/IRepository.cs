// hotel_restoraunt/Data/Interfaces/IRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}