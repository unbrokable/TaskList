using AutoMapper;
using MediatR;
using System.Text.Json.Serialization;

namespace TaskList.Application.TaskLists.Queries.GetTaskLists
{
    public class GetTaskListsQuery : IRequest<PageableResult<TaskListDto>>
    {
        [JsonIgnore]
        public required string UserId { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }

    public class GetTaskListsQueryHandler : IRequestHandler<GetTaskListsQuery, PageableResult<TaskListDto>>
    {
        private readonly ITaskListRepository repository;
        private readonly IMapper _mapper;

        public GetTaskListsQueryHandler(ITaskListRepository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

        public async Task<PageableResult<TaskListDto>> Handle(GetTaskListsQuery request, CancellationToken cancellationToken)
        {
            return _mapper
                .Map<PageableResult<TaskListDto>>(await repository.GetTaskListsAsync(request.UserId, request.Offset, request.Limit, cancellationToken));
        }
    }
}
