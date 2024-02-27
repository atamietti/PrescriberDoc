using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;

namespace PrescriberDocAPI.Patients.Domain
{
    public class CrudBase : EntityBase
    {
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        public virtual string Identification { get; set; }

        public static Type[] RegisteredTypes => Assembly.GetExecutingAssembly().GetTypes()?
                    .Where(t => t.IsSubclassOf(typeof(CrudBase)))?.ToArray() ?? Array.Empty<Type>();


        public static void MapType<T>(WebApplication app) where T : CrudBase
        {
            Type type = typeof(T);

            app.MapGet($"api/{type.Name.ToLower()}/" + "{id}", async (string id, IRepository<T> service) =>
            {
                var result = await service.Get(id);
                return string.IsNullOrEmpty(result.Id) ? Results.NotFound(id) : Results.Ok(result);
            })
            .RequireAuthorization()
            .WithTags($"{type.Name}");

            app.MapGet($"api/{type.Name.ToLower()}s", async (IRepository<T> service) =>
            {
                var result = await service.Get();
                return !result.Any() ? Results.NotFound() : Results.Ok(result);
            })
           .RequireAuthorization()
           .WithTags($"{type.Name}");

            app.MapPost($"api/{type.Name.ToLower()}/", async (T request, IRepository<T> service) =>
            {
                if (await service.Any(request.Identification, nameof(request.Identification))) 
                    return Results.Problem(
                        detail: $"{typeof(T).Name} {request.Name} id {request.Id} Already exists.",
                        statusCode: 500);

                var result = await service.Create(request);
                return result.Success ? Results.Ok(result) : Results.StatusCode(500);
            })
           .RequireAuthorization()
           .WithTags($"{type.Name}");

            app.MapPut($"api/{type.Name.ToLower()}/" + "{id}", async (string id, T updatedBlock, IRepository<T> service) =>
            {
                var result = await service.Update(id, updatedBlock);
                return result.Success ? Results.Ok("Success") : Results.StatusCode(500);
            })
           .RequireAuthorization()
           .WithTags($"{type.Name}");

            app.MapDelete($"api/{type.Name.ToLower()}/" + "{id}", async (string id, IRepository<T> service) =>
            {
                var result = await service.Delete(id);
                return result.Success ? Results.Ok("Success") : Results.StatusCode(500);
            })
           .RequireAuthorization()
           .WithTags($"{type.Name}");
        }

    }
}
