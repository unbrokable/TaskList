using MongoDB.Bson.Serialization.Attributes;

namespace TaskList.Domain.Common;

public abstract class BaseAuditableEntity 
{
    public DateTimeOffset Created { get; set; }

    public DateTimeOffset LastModified { get; set; }
}
