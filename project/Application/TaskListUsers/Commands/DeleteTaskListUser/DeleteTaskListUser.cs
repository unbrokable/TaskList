using MediatR;
using System.Text.Json.Serialization;

namespace TaskList.Application.TaskLists.Commands.DeleteTaskListUser;

public record class DeleteTaskListUserCommand : IRequest
{
    [JsonIgnore]
    public string CurrentUserId { get; set; } = string.Empty;

    [JsonIgnore]
    public string UserId {  get; set; } = string.Empty;

    public required string TaskListId { get; set; }
}

public class DeleteTaskListCommandUserHandler : IRequestHandler<DeleteTaskListUserCommand>
{
    private readonly ITaskListUserRepository repository;
    private readonly IAccessService accessService;

    public DeleteTaskListCommandUserHandler(ITaskListUserRepository repository, IAccessService accessService)
    {
        this.repository = repository;
        this.accessService = accessService;
    }

    public async Task Handle(DeleteTaskListUserCommand request, CancellationToken cancellationToken)
    {
        await accessService.CheckAccessToTaskListAsync(request.TaskListId, request.CurrentUserId, cancellationToken);

        await repository.DeleteUserFromTaskListAsync(request.TaskListId, request.UserId, cancellationToken);
    }
}
