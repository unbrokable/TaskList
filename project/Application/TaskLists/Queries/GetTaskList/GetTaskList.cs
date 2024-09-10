using AutoMapper;
using MediatR;

namespace TaskList.Application.TaskLists.Queries.GetTaskList
{
    public class GetTaskListQuery : IRequest<TaskListFullDto>
    {
        public required string UserId { get; set; }
        public required string TaskListId { get; set; }
    }

    public class GetTaskListQueryHandler : IRequestHandler<GetTaskListQuery, TaskListFullDto>
    {
        private readonly ITaskListRepository repository;
        private readonly IAccessService accessService;
        private readonly IMapper mapper;

        public GetTaskListQueryHandler(ITaskListRepository repository, IAccessService accessService, IMapper mapper)
        {
            this.repository = repository;
            this.accessService = accessService; 
            this.mapper = mapper;
        }

        public async Task<TaskListFullDto> Handle(GetTaskListQuery request, CancellationToken cancellationToken)
        {
            var taskList = await repository.GetByIdAsync(request.TaskListId, cancellationToken);

            Guard.Against.NotFound(request.TaskListId, taskList);

            accessService.CheckAccessToTaskList(taskList, request.UserId);

            return mapper.Map<TaskListFullDto>(taskList);
        }
    }
}
