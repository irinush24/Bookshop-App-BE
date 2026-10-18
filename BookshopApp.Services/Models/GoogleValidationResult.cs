namespace BookshopApp.Services.Models;

public class GoogleValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? GoogleId { get; set; }
}
