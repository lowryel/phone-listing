using System.Collections;

namespace PhoneListingAPI.PhoneListingDto;

public record class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public ICollection<Listing>? Listings { get; set; }

    public string GetUsername()
    {
        return Username;
    }
}
