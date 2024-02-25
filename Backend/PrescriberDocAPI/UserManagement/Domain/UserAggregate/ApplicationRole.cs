using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace PrescriberDocAPI.UserManagement.Domain.UserAggregate;

[CollectionName("roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{

}
