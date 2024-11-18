using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PhoneListingAPI.Models;

public class Listing
{
    [Key]
    public Guid ListingId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public User? User { get; set; }  // Navigation property

    [JsonIgnore]
    public ICollection<Image>? Images { get; set; }  // One-to-many relationship with images

    // Many-to-many relationship with categories through ListingCategory
    [JsonIgnore]
    public ICollection<ListingCategory>? Categories { get; set; }
}

