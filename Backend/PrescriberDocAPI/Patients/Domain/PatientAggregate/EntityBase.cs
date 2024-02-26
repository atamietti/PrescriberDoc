using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PrescriberDocAPI.Patients.Domain;

public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string Id { get; set; } = string.Empty;

    [JsonIgnore]
    [BsonIgnore]
    public string Message { get; set; } = string.Empty;

    [JsonIgnore]
    [BsonIgnore]
    public bool Success { get; set; } = true;


    public static T CreateErrorMessage<T>(string message, Exception? ex = null) where T : EntityBase
    {
        var response = Activator.CreateInstance<T>();
        response.Success = false;
        response.Message = $"{message} {Environment.NewLine}. {ex?.Message ?? string.Empty}";
        return response;
    }
}
