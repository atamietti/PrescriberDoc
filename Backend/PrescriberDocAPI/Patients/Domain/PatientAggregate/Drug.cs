using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PrescriberDocAPI.Patients.Domain;
[BsonIgnoreExtraElements]
public class Drug : CrudBase
{
    [JsonPropertyName("IdentificationDrugID")]
    public override string Identification { get => base.Identification; set => base.Identification = value; }
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}