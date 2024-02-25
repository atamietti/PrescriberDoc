using PrescriberDocAPI.Patients.Domain;
using System.Text.Json.Serialization;

namespace PrescriberDocAPI.Patients.Application;

public class DrugDTO : EntityBase
{
    [JsonIgnore]
    public override string Id { get => base.Id; set => base.Id = value; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

}
