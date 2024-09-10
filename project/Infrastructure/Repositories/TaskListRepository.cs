using MongoDB.Driver;
using TaskList.Application.Common.Interfaces;
using TaskList.Domain.Common;
using TaskList.Domain.Entities;

namespace TaskList.Infrastructure.Data
{
    public class TaskListRepository : GenericRepository<string, TaskListDb>, ITaskListRepository
    {
        public TaskListRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }

        public async Task<PageableResult<TaskListDb>> GetTaskListsAsync(string userId, int? offset, int? limit, CancellationToken cancellationToken)
        {
            var filter = Builders<TaskListDb>.Filter.Or(
               Builders<TaskListDb>.Filter.Eq(t => t.OwnerId, userId),
               Builders<TaskListDb>.Filter.ElemMatch(t => t.Users, u => u.UserId == userId)
            );

            var taskLists = await collection
                .Find(filter)
                .SortByDescending(t => t.Created)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync(cancellationToken);

            var totalDocuments = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            return new PageableResult<TaskListDb>
            {
                Items = taskLists,
                Total = totalDocuments,
                Limit = limit,
                Offset = offset
            };
        }

        public async Task AddUserToTaskListAsync(string taskListId, TaskListUserDb user, CancellationToken cancellationToken)
        {
            user.Created = DateTimeOffset.UtcNow;
            user.LastModified = DateTimeOffset.UtcNow;

            var filter = Builders<TaskListDb>.Filter.Eq(t => t.Id, taskListId) &
                Builders<TaskListDb>.Filter.ElemMatch(t => t.Users, u => u.UserId == user.UserId);

            var existingUser = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            if (existingUser == null)
            {
                var update = Builders<TaskListDb>.Update.Push(t => t.Users, user);
                await collection.UpdateOneAsync(t => t.Id == taskListId, update, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<TaskListUserDb>> GetUsersInTaskListAsync(string taskListId, CancellationToken cancellationToken)
        {
            var filter = Builders<TaskListDb>.Filter.And(
                Builders<TaskListDb>.Filter.Eq(t => t.Id, taskListId)
            );

            var taskList = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            return taskList?.Users ?? Enumerable.Empty<TaskListUserDb>();
        }

        public async Task DeleteUserFromTaskListAsync(string taskListId, string userId, CancellationToken cancellationToken)
        {
            var update = Builders<TaskListDb>.Update.PullFilter(t => t.Users, u => u.UserId == userId);
            await collection.UpdateOneAsync(t => t.Id == taskListId, update, cancellationToken: cancellationToken);
        }
    }
}
