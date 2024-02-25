using PrescriberDocAPI.Patients.Domain;
using System.Text.Json.Serialization;

namespace PrescriberDocAPI.Patients.Application
{
    public class CreateDrugRequest : EntityBase
    {
        [JsonIgnore]
        public override string Id { get => base.Id; set => base.Id = value; }

    }
}
