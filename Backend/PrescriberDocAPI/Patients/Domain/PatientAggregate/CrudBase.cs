using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace PrescriberDocAPI.Patients.Domain
{
    public class CrudBase : EntityBase
    {

        private static AuthorizeAttribute authorizedAttribute => new AuthorizeAttribute { Roles = "DOCTOR" };
        public static Type[] RegisteredTypes => Assembly.GetExecutingAssembly().GetTypes()?
                    .Where(t => t.IsSubclassOf(typeof(CrudBase)))?.ToArray() ?? Array.Empty<Type>();


        public static void MapType<T>(WebApplication app) where T : CrudBase
        {
            Type type = typeof(T);

            app.MapGet($"/{type.Name.ToLower()}" + "{id}", async (string id, IRepository<T> service) =>
            {
                var result = await service.Get(id);
                return string.IsNullOrEmpty(result.Id) ? Results.NotFound(id) : Results.Ok(result);
            })
            .RequireAuthorization(authorizedAttribute)
            .WithTags($"{type.Name}");

            app.MapGet($"/{type.Name.ToLower()}", async (IRepository<T> service) =>
            {
                var result = await service.Get();
                return !result.Any()? Results.NotFound() : Results.Ok(result);
            })
           .RequireAuthorization(authorizedAttribute)
           .WithTags($"{type.Name}");

            app.MapPost($"/{type.Name.ToLower()}/", async (T request, IRepository<T> service) =>
            {
             
                var result = await service.Create(request);
                return result ? Results.Ok(result) : Results.StatusCode(500);
            })
           .RequireAuthorization(authorizedAttribute)
           .WithTags($"{type.Name}");

            app.MapPut($"/{type.Name.ToLower()}/", async (string id, T updatedBlock, IRepository<T> service) =>
            {
                var result = await service.Update(id,  updatedBlock);
                return result ? Results.Ok("Success") : Results.StatusCode(500);
            })
           .RequireAuthorization(authorizedAttribute)
           .WithTags($"{type.Name}");

            app.MapDelete($"/{type.Name.ToLower()}/", async (string id , IRepository<T> service) =>
            {
                var result = await service.Delete(id);
                return result ? Results.Ok("Success") : Results.StatusCode(500);
            })
           .RequireAuthorization(authorizedAttribute)
           .WithTags($"{type.Name}");
        }

    }
}
