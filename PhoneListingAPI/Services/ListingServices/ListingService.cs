using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilePhoneListing.DBContext;
using PhoneListingAPI.PhoneListingDto;
using PhoneListingAPI.PhoneListingDto.ListingDto;
namespace PhoneListingAPI.Services.ListingServices;

public class ListingServise : IListingService
{
    private readonly MobilePhoneDbContext _mobilePhoneDbContext;

    public ListingServise(MobilePhoneDbContext mobilePhoneDbContext)
    {
        _mobilePhoneDbContext = mobilePhoneDbContext;
    }

    // public async Task<Listing> CreateNewListing([FromBody] ListingDto listingDto)
    // {
    //     // Fetch the user by UserId
    //     var user = await _mobilePhoneDbContext.Users.FindAsync(listingDto.UserId) ?? throw new Exception("User not found.");

    //     var listing = new Listing
    //     {
    //         ListingId = Guid.NewGuid(),
    //         Title = listingDto.Title,
    //         Description = listingDto.Description,
    //         Price = listingDto.Price,
    //         CreatedAt = DateTime.Now.ToLocalTime(),
    //         UserId = user.UserId,   // Attach the listing to the user
    //         User = user             // Set the navigation property
    //     };

    //     await _mobilePhoneDbContext.Listings.AddAsync(listing);
    //     await _mobilePhoneDbContext.SaveChangesAsync();
    //     return listing;
    // }

    public async Task<Listing> CreateListing(Guid userId, ListingDto listingDto)
    {
        // Check if the user exists without loading all their listings
        var userExists = await _mobilePhoneDbContext.Users.AnyAsync(u => u.UserId == userId);

        if (!userExists)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        // Create a new listing
        var listing = new Listing
        {
            ListingId = Guid.NewGuid(),
            Description = listingDto.Description,
            Title = listingDto.Title,
            Price = listingDto.Price,
            UserId = userId,
            CreatedAt = DateTime.UtcNow // Set the creation time
        };

        // Add the listing to the context
        await _mobilePhoneDbContext.Listings.AddAsync(listing);

        // Save changes to the database
        await _mobilePhoneDbContext.SaveChangesAsync();

        return listing;
    }



    public async Task<IEnumerable<ListingDto>> GetAllListings()
    {
        var listings = await _mobilePhoneDbContext.Listings.ToListAsync();

        if (listings == null || listings.Count == 0)
        {
            throw new Exception("No listings found");
        }

        return listings.Select(l => new ListingDto
        {
            ListingId = l.ListingId,
            Title = l.Title,
            Description = l.Description,
            Price = l.Price,
            UserId = l.UserId
        });
    }

    public async Task<ListingDto> GetListingAsync([FromRoute] Guid listingId)
    {
        var listing = await _mobilePhoneDbContext.Listings.FindAsync(listingId) ?? throw new Exception($"Listing with Id: {listingId} not found");
        return new ListingDto
        {
            ListingId = listing.ListingId,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            UserId = listing.UserId
        };
    }

    public async Task<ListingDto> UpdateListing([FromRoute] Guid listingId, [FromBody] UpdateListingDto updateListing)
    {
        var listing = await _mobilePhoneDbContext.Listings.FindAsync(listingId);
        if (listing == null)
        {
            throw new Exception($"Listing with Id: {listingId} not found");
        }

        // Update only the properties that are not null in the DTO
        if (updateListing.Title != null)
        {
            listing.Title = updateListing.Title;
        }
        if (updateListing.Description != null)
        {
            listing.Description = updateListing.Description;
        }
        if (updateListing.Price.HasValue)
        {
            listing.Price = updateListing.Price.Value;
        }

        _mobilePhoneDbContext.Listings.Update(listing);
        await _mobilePhoneDbContext.SaveChangesAsync();

        return new ListingDto
        {
            ListingId = listing.ListingId,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            UserId = listing.UserId
        };
    }

    public async Task<Listing> DeleteListing([FromRoute] Guid listingId)
    {
        var listing = await _mobilePhoneDbContext.Listings.FindAsync(listingId);
        if (listing == null)
        {
            return null!;
        }

        _mobilePhoneDbContext.Listings.Remove(listing);
        await _mobilePhoneDbContext.SaveChangesAsync();
        return listing;
    }
}

public interface IListingService
{
    Task<IEnumerable<ListingDto>> GetAllListings();
    Task<Listing> CreateListing(Guid userId, ListingDto listingDto);
    Task<ListingDto> GetListingAsync([FromRoute] Guid listingId);
    Task<ListingDto> UpdateListing([FromRoute] Guid listingId, [FromBody] UpdateListingDto updateListing);
    Task<Listing> DeleteListing([FromRoute] Guid listingId);
}


