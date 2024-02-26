using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PrescriberDocAPI.Patients.Domain;
[BsonIgnoreExtraElements]

public class Patient : CrudBase
{
    [JsonPropertyName("IdentificationCard")]
    public override string Identification { get => base.Identification; set => base.Identification = value; }
    public string DoctorId { get; set; } = string.Empty;
    
    public List<string> Drugs { get; set; } = new List<string>();
}
