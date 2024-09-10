namespace TaskList.Application.Services;

public interface IAccessService
{
    void CheckAccessToTaskList(TaskListDb taskListDb, string userId);

    void CheckDeleteAccessToTaskList(TaskListDb taskListDb, string userId);

    Task CheckAccessToTaskListAsync(string taskListId, string userId, CancellationToken cancellationToken = default);
}
