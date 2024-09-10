namespace TaskList.Application.Services;

public class AccessService : IAccessService
{

    private readonly ITaskListRepository _taskListRepository;

    public AccessService(ITaskListRepository taskListRepository)
    {
        _taskListRepository = taskListRepository;
    }

    public async Task CheckAccessToTaskListAsync(string taskListId, string userId, CancellationToken cancellationToken = default)
    {
        var taskList = await _taskListRepository.GetByIdAsync(taskListId, cancellationToken);

        Guard.Against.NotFound(taskListId, taskList);

        CheckAccessToTaskList(taskList, userId);

    }

    public void CheckAccessToTaskList(TaskListDb taskListDb, string userId)
    {
        bool isHasAccess = taskListDb.OwnerId == userId || taskListDb.Users.Any(u => u.UserId == userId);

        Guard.Against.AgainstForbidden(isHasAccess);

    }

    public void CheckDeleteAccessToTaskList(TaskListDb taskListDb, string userId)
    {
        bool isHasAccess = taskListDb.OwnerId == userId;

        Guard.Against.AgainstForbidden(isHasAccess);
    }
}
