using System.ComponentModel.DataAnnotations;

namespace PhoneListingAPI.Models;

public class Category  // more like the various brands
{
    [Key]
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = String.Empty;
    
    // Represents the many-to-many relationship with Listing through ListingCategory
    public ICollection<ListingCategory>? Listings { get; set; }
}
