using AutoMapper;
using TaskList.Application.TaskLists.Queries.GetTaskList;
using TaskList.Application.TaskLists.Queries.GetTaskLists;

namespace TaskList.Application.Common.Profiles
{
    public class TaskListProfile : Profile
    {
        public TaskListProfile() 
        {
            CreateMap<TaskListDb, TaskListDto>();
            CreateMap<TaskListDb, TaskListFullDto>();

            CreateMap<PageableResult<TaskListDb>, PageableResult<TaskListDto>>();
        }

    }
}
