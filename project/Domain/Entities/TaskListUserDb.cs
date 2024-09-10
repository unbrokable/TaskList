namespace TaskList.Domain.Entities;
public class TaskListUserDb : BaseEntity<string>
{
    public required string UserId { get; set; }

    public required string TaskListId { get; set; }
}
