using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class SignupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/signup", async (
            [FromBody] SignupRequest body,
            UserManager<AppUser> userManager) =>
        {
            if (string.IsNullOrWhiteSpace(body.Email) || string.IsNullOrWhiteSpace(body.Password))
                return Results.BadRequest("Email and password are required.");

            var user = new AppUser { UserName = body.Email, Email = body.Email };
            var result = await userManager.CreateAsync(user, body.Password);

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok("User created");
        });
    }
}

public class SignupRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}