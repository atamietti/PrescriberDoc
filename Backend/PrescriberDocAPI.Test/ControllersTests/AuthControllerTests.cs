using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PrescriberDocAPI.Controllers;
using PrescriberDocAPI.Shared.Domain;
using PrescriberDocAPI.UserManagement.Application;
using PrescriberDocAPI.UserManagement.Domain.UserAggregate;
using System.Numerics;

namespace PrescriberDocAPI.Test.ControllersTests
{
    public class AuthControllerTests
    {
        AuthController _controller;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<RoleManager<ApplicationRole>> _roleManager;
        private readonly UserConfig _userConfig;


        public AuthControllerTests()
        {
            _userConfig = new UserConfig { IssuerSigningKey = "0235a3cf-38cb-4fe0-8be2-ea7758d11ec1" };
            _roleManager = MockRoleManager();
            _userManager = MockUserManager(AuthMock.Users);
            _controller = new AuthController(_userManager.Object, _roleManager.Object, _userConfig);

        }
        public Mock<RoleManager<ApplicationRole>> MockRoleManager()
        {
            var rolestore = new Mock<IRoleStore<ApplicationRole>>();
            rolestore.Setup(_ => _.CreateAsync(It.IsAny<ApplicationRole>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
            rolestore.Setup(_ => _.CreateAsync(new ApplicationRole { Name = "USER" }, It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed());

            return new Mock<RoleManager<ApplicationRole>>(rolestore.Object, null, null, null, null);

        }
        public Mock<UserManager<ApplicationUser>> MockUserManager(List<ApplicationUser> ls)
        {
            var mgr = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            mgr.Setup(_ => _.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(_ => _.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => ls.Add(x));
            mgr.Setup(_ => _.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(_ => _.FindByEmailAsync(AuthMock.User.Email)).ReturnsAsync(AuthMock.User);
            mgr.Setup(_ => _.AddToRoleAsync(It.IsAny<ApplicationUser>(), "DOCTOR")).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
            mgr.Setup(x => x.CheckPasswordAsync(AuthMock.User, AuthMock.RegisterRequest.Password)).ReturnsAsync(true);
            mgr.Setup(_ => _.CreateAsync(AuthMock.User, AuthMock.RegisterRequest.Password)).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(_ => _.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "Doctor" });


            return mgr;
        }

        [Fact]
        public async Task Register_ValidData()
        {
            var userRequest = new RegisterRequest
            {
                Email = "teste2@teste.com",
                Fullname = "User Teste2",
                Password = "password",
                ConfirmPassword = "password",
                Username = "teste"
            };

            var response = await _controller.Register(userRequest);
            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        }

        [Fact]
        public async Task Register_InvalidData()
        {

            var response = await _controller.Register(AuthMock.RegisterRequest);
            var failResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(StatusCodes.Status400BadRequest, failResult.StatusCode);

        }

        [Fact]
        public async Task Login_Valid()
        {
            var response = await _controller.Login(AuthMock.LoginRequest);
            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }


        [Fact]
        public async Task Login_Invalid()
        {
            var loginrequest = new LoginRequest
             {
                 Email = "teste2@teste.com",
                 Password = Guid.NewGuid().ToString(),
             };
            var response = await _controller.Login(loginrequest);
            var okResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }


        [Fact]
        public async Task CreateRole_Invalid()
        {
            var response = await _controller.CreateRole(new CreateRoleRequest { Role = "USER" });
            var okResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }
    }
}
