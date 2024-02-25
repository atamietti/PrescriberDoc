using MongoDB.Bson.Serialization.Attributes;

namespace PrescriberDocAPI.Patients.Domain;
[BsonIgnoreExtraElements]

public class Patient : CrudBase
{
   
    public string DoctorId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Drugs { get; set; } = new List<string>();
}
