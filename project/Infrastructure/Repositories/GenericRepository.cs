using MongoDB.Bson;
using MongoDB.Driver;
using TaskList.Application.Common.Interfaces;
using TaskList.Domain.Common;

namespace TaskList.Infrastructure.Data
{
    public class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        protected readonly IMongoCollection<TEntity> collection;

        public GenericRepository(IMongoDatabase database, string collectionName)
        {
            this.collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await collection.Find(new BsonDocument()).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await collection.Find(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id?.ToString()))).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TKey> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.Created = DateTime.UtcNow;
            entity.LastModified = DateTime.UtcNow;

            await collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

            return entity.Id;
        }

        public async Task UpdateAsync(TKey id, TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.LastModified = DateTime.UtcNow;

            await collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id?.ToString())), entity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id?.ToString())), cancellationToken: cancellationToken);
        }
    }
}
