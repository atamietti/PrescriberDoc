using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace PrescriberDocAPI.UserManagement.Domain.UserAggregate;

[CollectionName("users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public string FullName { get; set; }    = string.Empty;
}
