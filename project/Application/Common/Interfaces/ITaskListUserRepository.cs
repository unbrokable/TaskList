namespace TaskList.Application.Common.Interfaces;

public interface ITaskListUserRepository 
{
    Task AddUserToTaskListAsync(string taskListId, TaskListUserDb user, CancellationToken cancellationToken);

    Task<IEnumerable<TaskListUserDb>> GetUsersInTaskListAsync(string taskListId, CancellationToken cancellationToken);

    Task DeleteUserFromTaskListAsync(string taskListId, string userId, CancellationToken cancellationToken);
}
