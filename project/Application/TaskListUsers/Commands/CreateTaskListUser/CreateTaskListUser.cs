using MediatR;
using System.Text.Json.Serialization;

namespace TaskList.Application.TaskLists.Commands.CreateTaskListUser;

public record class CreateTaskListUserCommand : IRequest<Unit>
{
    [JsonIgnore]
    public string CurrentUserId { get; set; } = string.Empty;

    public required string UserId { get; set; }

    [JsonIgnore]
    public string TaskListId { get; set; } = string.Empty;
}

public class CreateTaskListUserCommandHandler : IRequestHandler<CreateTaskListUserCommand, Unit>
{
    private readonly ITaskListUserRepository repository;
    private readonly IAccessService accessService;

    public CreateTaskListUserCommandHandler(ITaskListUserRepository repository, IAccessService accessService)
    {
        this.repository = repository;
        this.accessService = accessService;
    }

    public async Task<Unit> Handle(CreateTaskListUserCommand request, CancellationToken cancellationToken)
    {
        await accessService.CheckAccessToTaskListAsync(request.TaskListId, request.CurrentUserId, cancellationToken);

        await repository.AddUserToTaskListAsync(request.TaskListId, new TaskListUserDb() 
        {
            TaskListId = request.TaskListId,
            UserId = request.UserId,
        }, cancellationToken);

        return Unit.Value;
    }
}