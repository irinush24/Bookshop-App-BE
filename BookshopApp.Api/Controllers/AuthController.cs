using Bookshop_App_BE.ApiModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using BookshopApp.Services.Interfaces;

namespace Bookshop_App_BE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    // Triggers the Google login screen
    [HttpGet("login")]
    public IActionResult Login()
    {
        // After logging in, Google will redirect back to the /api/auth/callback endpoint
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("Callback") };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return BadRequest("Google authentication failed");

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
        {
            claim.Type,
            claim.Value
        });

        return Ok(claims);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyToken([FromBody] GoogleTokenRequest request)
    {
        if(string.IsNullOrEmpty(request.Token))
        {
            return BadRequest("Token is missing");
        }

        var result = await authService.ValidateGoogleTokenAsync(request.Token);

        if(!result.IsValid)
            return Unauthorized(result.ErrorMessage);

        return Ok(new
        {
            name = result.Name,
            email = result.Email,
            googleId = result.GoogleId,
            token = "simulated-dotnet-jwt-token"
        });
    }
}
