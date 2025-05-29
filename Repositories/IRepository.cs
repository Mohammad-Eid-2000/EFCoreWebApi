using EFCoreWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCoreWebApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?>GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task <bool> DeleteAsync(int id);
        Task<IEnumerable<T>> Search(string propertyName , string searchText);
        Task<T?> SearchSingle(string searchText);

    }
}