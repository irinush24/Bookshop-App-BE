using BookshopApp.Services.Interfaces;
using BookshopApp.Services.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace BookshopApp.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GoogleValidationResult> ValidateGoogleTokenAsync(string token)
    {
        try
        {
            var clientId = _configuration["Authentication:Google:ClientId"];
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            return new GoogleValidationResult
            {
                IsValid = true,
                Name = payload.Name,
                Email = payload.Email,
                GoogleId = payload.Subject,
            };
        }
        catch (InvalidJwtException ex)
        {
            return new GoogleValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Invalid Google token: {ex.Message}",
            };
        }
    }
}
