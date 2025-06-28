using System.ComponentModel.DataAnnotations;
namespace Invento.Shared.Models;

public class User
{
    public int Id { get; set; }
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int? BusinessId { get; set; }
    public Business? Business { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

public enum UserRole { Owner = 1, Manager = 2, Staff = 3 }
