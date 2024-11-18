using System.ComponentModel.DataAnnotations;
using PhoneListingAPI.Models;

public class ListingCategory
{
    [Key]
    public Guid ListingId { get; set; }
    public Listing? Listing { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}
