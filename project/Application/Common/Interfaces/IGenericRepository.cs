namespace TaskList.Application.Common.Interfaces;

public interface IGenericRepository<TPrimaryKey, TEntity> where TEntity : BaseEntity<TPrimaryKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
    Task<TPrimaryKey> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TPrimaryKey id, TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
}
