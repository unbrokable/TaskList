using MediatR;

namespace TaskList.Application.TaskLists.Commands.DeleteTaskList;

public record class DeleteTaskListCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;

    public required string TaskListId { get; set; }
}

public class DeleteTaskListCommandHandler : IRequestHandler<DeleteTaskListCommand, Unit>
{
    private readonly ITaskListRepository repository;
    private readonly IAccessService accessService;

    public DeleteTaskListCommandHandler(ITaskListRepository repository, IAccessService accessService)
    {
        this.repository = repository;
        this.accessService = accessService;
    }

    public async Task<Unit> Handle(DeleteTaskListCommand request, CancellationToken cancellationToken)
    {
        var taskList = await repository.GetByIdAsync(request.TaskListId, cancellationToken);

        Guard.Against.NotFound(request.TaskListId, taskList);

        accessService.CheckDeleteAccessToTaskList(taskList, request.UserId);

        await repository.DeleteAsync(request.TaskListId, cancellationToken);

        return Unit.Value;
    }
}
