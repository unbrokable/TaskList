namespace TaskList.Application.Common.Interfaces;

public interface ITaskListRepository : IGenericRepository<string, TaskListDb>, ITaskListUserRepository
{
    Task<PageableResult<TaskListDb>> GetTaskListsAsync(string userId, int? offset, int? limit, CancellationToken cancellationToken);
}
