using MediatR;
using System.Text.Json.Serialization;
using TaskList.Application.Common.Model;

namespace TaskList.Application.TaskLists.Commands.CreateTaskList;

public record class CreateTaskListCommand : IRequest<CreatedResponse>
{
    public required string Title { get; set; }

    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}

public class CreateTaskListCommandHandler : IRequestHandler<CreateTaskListCommand, CreatedResponse>
{

    private readonly ITaskListRepository repository;

    public CreateTaskListCommandHandler(ITaskListRepository repository)
    {
        this.repository = repository;
    }

    public async Task<CreatedResponse> Handle(CreateTaskListCommand request, CancellationToken cancellationToken)
    {
        var id = await repository.AddAsync(new TaskListDb
        {
            Title = request.Title,
            OwnerId = request.UserId
        }, cancellationToken);

        return new CreatedResponse() { Id = id };
    }
}