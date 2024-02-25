using MongoDB.Bson.Serialization.Attributes;

namespace PrescriberDocAPI.Patients.Domain;
[BsonIgnoreExtraElements]
public class Medicine : CrudBase
{

    [BsonRequired]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}