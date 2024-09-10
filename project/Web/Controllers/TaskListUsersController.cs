using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskList.Application.TaskLists.Commands.CreateTaskListUser;
using TaskList.Application.TaskLists.Commands.DeleteTaskListUser;
using TaskList.Application.TaskLists.Queries.GetTaskListUsers;
using TaskList.Web.Extention;

namespace TaskList.Web.Controllers
{
    [Route("v1/task-lists")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class TaskListUsersController : ControllerBase
    {
        private readonly ISender _sender;

        public TaskListUsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{taskListId}/users", Name = "Get Task List Users")]
        public async Task<IActionResult> GetTaskListUsersAsync([FromRoute] string taskListId, CancellationToken cancellationToken = default)
        {
            var query = new GetTaskListUsersQuery() { TaskListId = taskListId, UserId = HttpContext.GetUserId() };

            return Ok(await _sender.Send(query, cancellationToken));
        }

        [HttpPost("{taskListId}/users", Name = "Create Task List User")]
        public async Task<IActionResult> GetTaskListUserAsync([FromRoute] string taskListId, [FromBody] CreateTaskListUserCommand command, CancellationToken cancellationToken = default)
        {
            command.CurrentUserId = HttpContext.GetUserId();
            command.TaskListId = taskListId;

            return Created(string.Empty, await _sender.Send(command, cancellationToken));
        }


        [HttpDelete("{taskListId}/users/{userId}", Name = "Delete Task List User")]
        public async Task<IActionResult> DeleteTaskListUserAsync([FromRoute] string taskListId, [FromRoute] string userId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteTaskListUserCommand 
            { 
                TaskListId = taskListId, 
                CurrentUserId = HttpContext.GetUserId(),
                UserId = HttpContext.GetUserId()
            };

            await _sender.Send(command, cancellationToken);

            return NoContent();
        }
    }
}