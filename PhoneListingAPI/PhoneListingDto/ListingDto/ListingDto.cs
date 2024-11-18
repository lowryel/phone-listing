using System;

namespace PhoneListingAPI.PhoneListingDto.ListingDto;

public class ListingDto
{
    public Guid ListingId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid UserId { get; set; }
}


public class UpdateListingDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
}


