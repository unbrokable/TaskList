using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskList.Domain.Common;

public abstract class BaseEntity<TPrimaryKey> : BaseAuditableEntity 
{
    [BsonRepresentation(BsonType.ObjectId)]
    public TPrimaryKey Id { get; set; } = default!;
}
