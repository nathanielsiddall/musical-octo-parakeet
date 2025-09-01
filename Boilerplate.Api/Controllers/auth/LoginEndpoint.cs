using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Boilerplate.Api.Auth;

public class LoginEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            [FromBody] LoginRequest body,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration config) =>
        {
            var user = await userManager.FindByEmailAsync(body.Email);
            if (user == null) return Results.Unauthorized();

            var result = await signInManager.CheckPasswordSignInAsync(user, body.Password, false);
            if (!result.Succeeded) return Results.Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email ?? user.UserName ?? "unknown")
            };

            var secret = config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(secret))
                return Results.Problem("JWT secret key is not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) })
                .WithName("Login")
                .WithTags("Auth")
                .WithOpenApi();
        });
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}