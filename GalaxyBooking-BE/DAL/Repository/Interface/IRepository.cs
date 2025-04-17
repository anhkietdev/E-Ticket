using DAL.Common;
using System.Linq.Expressions;

namespace DAL.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<ICollection<T>> GetPagingAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool tracked = false,
            Expression<Func<T, object>>? orderBy = null,
            SortDirection sortDirection = SortDirection.Descending,
            int pageNumber = 1,
            int pageSize = 5
        );

        Task<ICollection<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            Expression<Func<T, object>>? orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            bool tracked = false
        );

        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);


        Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            string? includeProperties = null,
            bool tracked = false
        );


        Task AddAsync(T entity);


        Task AddRange(IEnumerable<T> entities);


        Task RemoveAsync(params T[] entities);


        Task RemoveRange(IEnumerable<T> entities);


        Task UpdateAsync(T entity);


        Task UpdateRange(IEnumerable<T> entities);


        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    }
}
