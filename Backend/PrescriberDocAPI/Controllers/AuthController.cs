using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrescriberDocAPI.Shared.Domain;
using PrescriberDocAPI.UserManagement.Application;
using PrescriberDocAPI.UserManagement.Domain.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PrescriberDocAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserConfig _userConfig;

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, UserConfig userConfig)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userConfig = userConfig;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(RegisterResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await RegisterAsync(request);

        return result.Success ? Ok(result) : BadRequest(result.Message);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await LoginAsync(request);

        return result.Success ? Ok(result) : BadRequest(result.Message);
    }

    [HttpPost]
    [Route("roles/add")]
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        try
        {
            var appRole = new ApplicationRole { Name = request.Role };
            var createRole = await _roleManager.CreateAsync(appRole);
            if (!createRole?.Succeeded ?? true) return BadRequest(new { message = $"Create role failed. {string.Join(Environment.NewLine, createRole?.Errors?.Select(f => f.Description) ?? new string[] { string.Empty })}" });

            return Ok(new { message = "Role created successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Create role failed. {ex.Message}" });
        }
    }

    private async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var roleName = "DOCTOR";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null) return new RegisterResponse { Message = "User already exists.", Success = false };

            user = new ApplicationUser
            {
                FullName = request.Fullname,
                Email = request.Email,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                UserName = request.Email,
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded) return new RegisterResponse
            {
                Message = $"Create user failed. {createResult?.Errors.First()?.Description}",
                Success = false
            };


            var userInRole = await _userManager.AddToRoleAsync(user, roleName);
            if (!userInRole.Succeeded) return new RegisterResponse
            {
                Message = $"Create user succeded but could not add user to role. {string.Join(Environment.NewLine, userInRole?.Errors?.Select(f => f.Description) ?? new string[] { string.Empty })}",
                Success = false
            };

            return new RegisterResponse
            {
                Success = true,
                Message = "User registered successfully"
            };
        }
        catch (Exception ex)
        {

            return new RegisterResponse { Success = false, Message = ex.Message };

        }
    }

    private async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (string.IsNullOrWhiteSpace(user?.UserName)) return new LoginResponse { Message = "Invalid email/password", Success = false };

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!validPassword) return new LoginResponse { Message = "Invalid email/password", Success = false };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,  user.Id.ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_userConfig.IssuerSigningKey));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "Login Sucessful",
                Success = true,
                Email = user?.Email ?? string.Empty,
                UserId = user?.Id.ToString() ?? string.Empty,
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = ex.Message };
        }

    }
}
