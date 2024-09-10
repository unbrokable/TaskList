using AutoMapper;
using TaskList.Application.TaskLists.Queries.GetTaskListUsers;

namespace TaskList.Application.Common.Profiles
{
    public class TaskListUserProfile : Profile
    {
        public TaskListUserProfile() 
        {
            CreateMap<TaskListUserDb, TaskListUserDto>();
        }
    }
}
