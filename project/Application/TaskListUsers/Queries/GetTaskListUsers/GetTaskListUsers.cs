using AutoMapper;
using MediatR;
using System.Text.Json.Serialization;

namespace TaskList.Application.TaskLists.Queries.GetTaskListUsers
{
    public class GetTaskListUsersQuery : IRequest<IEnumerable<TaskListUserDto>>
    {
        public required string TaskListId { get; set; }

        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }

    public class GetTaskListUsersQueryHandler : IRequestHandler<GetTaskListUsersQuery, IEnumerable<TaskListUserDto>>
    {
        private readonly ITaskListUserRepository repository;
        private readonly IMapper mapper;
        private readonly IAccessService accessService;

        public GetTaskListUsersQueryHandler(ITaskListUserRepository repository, IMapper mapper, IAccessService accessService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.accessService = accessService;   
        }

        public async Task<IEnumerable<TaskListUserDto>> Handle(GetTaskListUsersQuery request, CancellationToken cancellationToken)
        {
            await accessService.CheckAccessToTaskListAsync(request.TaskListId, request.UserId, cancellationToken);

            return this.mapper
                .Map<IEnumerable<TaskListUserDto>>(await repository.GetUsersInTaskListAsync(request.TaskListId, cancellationToken));
        }
    }
}
