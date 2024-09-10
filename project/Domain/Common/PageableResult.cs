namespace TaskList.Domain.Common;

public class PageableResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; } = [];

    public long? Total { get; set; }

    public int? Limit { get; set; }

    public int? Offset { get; set; }
}