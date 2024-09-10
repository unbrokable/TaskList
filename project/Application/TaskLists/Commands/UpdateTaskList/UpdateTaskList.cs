using MediatR;
using System.Text.Json.Serialization;

namespace TaskList.Application.TaskLists.Commands.CreateTaskList;

public record class UpdateTaskListCommand : IRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    [JsonIgnore]
    public string TaskListId { get; set; } = string.Empty;
    public required string Title { get; set; }
}

public class UpdateTaskListCommandHandler : IRequestHandler<UpdateTaskListCommand>
{

    private readonly ITaskListRepository repository;
    private readonly IAccessService accessService;

    public UpdateTaskListCommandHandler(ITaskListRepository repository, IAccessService accessService)
    {
        this.repository = repository;
        this.accessService = accessService;
    }

    public async Task Handle(UpdateTaskListCommand request, CancellationToken cancellationToken)
    {
        var taskList = await repository.GetByIdAsync(request.TaskListId, cancellationToken);

        Guard.Against.NotFound(request.TaskListId, taskList);

        accessService.CheckAccessToTaskList(taskList, request.UserId);

        taskList.Title = request.Title;

        await repository.UpdateAsync(request.TaskListId, taskList, cancellationToken);
    }
}