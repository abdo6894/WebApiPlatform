

using ecommrece.sharedliberary.Responses;
using System.Linq.Expressions;

namespace ecommrece.sharedliberary.Interface
{
    public interface IgenericInterface<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<Response> CreateAsync(T entity);
        Task<T> FindByIdAsync(int id);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync( T entity);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicte); 
    }
}
