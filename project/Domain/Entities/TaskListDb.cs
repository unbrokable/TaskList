namespace TaskList.Domain.Entities;

public class TaskListDb : BaseEntity<string>
{
    public required string Title { get; set; }

    public required string OwnerId { get; set; }
    
    public IList<TaskListUserDb> Users { get; set; } = new List<TaskListUserDb>();
}
