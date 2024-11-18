public class Image
{
    public Guid ImageId { get; set; }
    public string? Url { get; set; }
    public Guid ListingId { get; set; }

    public Listing? Listing { get; set; }  // Navigation property
}
