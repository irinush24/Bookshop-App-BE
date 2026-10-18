using BookshopApp.Services.Models;

namespace BookshopApp.Services.Interfaces;

public interface IAuthService
{
    Task<GoogleValidationResult> ValidateGoogleTokenAsync(string token);
}
