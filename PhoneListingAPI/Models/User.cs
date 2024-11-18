using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PhoneListingAPI.Services;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public required string PasswordHash { get; set; }
    public bool Active { get; set; } = false;
    public bool IsDeleted { get; set;} = false;
    public ICollection<Listing>? Listings { get; set; }

    public void SetPassword(string password)
    {
        PasswordHash = PasswordHasher.HashPassword(password);
    }
}
