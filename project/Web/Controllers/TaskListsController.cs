using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskList.Application.TaskLists.Commands.CreateTaskList;
using TaskList.Application.TaskLists.Commands.DeleteTaskList;
using TaskList.Application.TaskLists.Queries.GetTaskList;
using TaskList.Application.TaskLists.Queries.GetTaskLists;
using TaskList.Web.Extention;

namespace TaskList.Web.Controllers
{
    [Route("v1/task-lists")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class TaskListsController : ControllerBase
    {
        private readonly ISender _sender;

        public TaskListsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id}", Name = "Get Task List")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var query = new GetTaskListQuery() { TaskListId = id, UserId = HttpContext.GetUserId() };

            return Ok(await _sender.Send(query, cancellationToken));
        }

        [HttpGet(Name = "Get Task Lists")]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery(Name = "offset")] int? offset,
            [FromQuery(Name = "limit")] int? limit = 10, 
            CancellationToken cancellationToken = default)
        {
            var query = new GetTaskListsQuery() { 
                Offset = offset,
                Limit = limit,
                UserId = HttpContext.GetUserId()
            };

            return Ok(await _sender.Send(query));
        }

        [HttpPost(Name = "Create Task List")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskListCommand command, CancellationToken cancellationToken = default)
        {
            command.UserId = HttpContext.GetUserId();

            return Created(string.Empty, await _sender.Send(command, cancellationToken));
        }

        [HttpPut("{id}", Name = "Update Task List")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateTaskListCommand command, CancellationToken cancellationToken = default)
        {
            command.TaskListId = id;
            command.UserId = HttpContext.GetUserId();

            await _sender.Send(command, cancellationToken);

            return Ok();
        }

        [HttpDelete("{id}", Name = "Delete Task List")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteTaskListCommand { TaskListId = id, UserId = HttpContext.GetUserId() };

            await _sender.Send(command, cancellationToken);

            return NoContent();
        }
    }
}