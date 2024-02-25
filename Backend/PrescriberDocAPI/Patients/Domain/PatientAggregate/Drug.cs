using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrescriberDocAPI.Patients.Domain;

public class Drug : CrudBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}