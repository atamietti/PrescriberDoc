using PrescriberDocAPI.UserManagement.Application;
using PrescriberDocAPI.UserManagement.Domain.UserAggregate;

namespace PrescriberDocAPI.Test
{
    public class AuthMock
    {
        public static string Pwd { get; set; } = Guid.NewGuid().ToString();
        public static RegisterRequest RegisterRequest { get; set; } = new RegisterRequest
        {
            Email = "teste@teste.com",
            Fullname = "User Teste",
            Password = Pwd,
            ConfirmPassword = Pwd,
            Username = "teste"
        };
        public static ApplicationUser User { get; set; } = new ApplicationUser
        {
            FullName = RegisterRequest.Fullname,
            Email = RegisterRequest.Email,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            UserName = RegisterRequest.Email,

        };

        public static List<ApplicationUser> Users = new List<ApplicationUser>
         {
            User
         };

        public static LoginRequest LoginRequest { get; set; } = new LoginRequest
        {
            Email = RegisterRequest.Email,
            Password = Pwd
        };
    }
}
