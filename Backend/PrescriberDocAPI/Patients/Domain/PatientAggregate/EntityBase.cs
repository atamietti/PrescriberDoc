using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrescriberDocAPI.Patients.Domain;

public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string Id { get; set; } = string.Empty;
}
