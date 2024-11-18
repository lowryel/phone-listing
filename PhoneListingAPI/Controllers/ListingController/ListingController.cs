using System;


using Microsoft.AspNetCore.Mvc;
using PhoneListingAPI.Services;
using PhoneListingAPI.PhoneListingDto;
using global::PhoneListingAPI.PhoneListingDto.ListingDto;
using PhoneListingAPI.Services.ListingServices;

namespace PhoneListingAPI.Controllers.ListingController;

[ApiController]
[Route("api/")]
public class ListingController : ControllerBase
{
    private readonly IListingService _listingService;

    public ListingController(IListingService listingService)
    {
        _listingService = listingService;
    }

    // GET: api/listing
    [HttpGet]
    [Route("listing/")]
    public async Task<IActionResult> GetAllListings()
    {
        var listings = await _listingService.GetAllListings();
        if (listings == null)
        {
            return NotFound("No listings found.");
        }

        return Ok(listings);
    }

    // GET: api/listing/{id}
    [HttpGet]
    [Route("{listingId}")]
    public async Task<IActionResult> GetListing([FromRoute] Guid listingId)
    {
        var listing = await _listingService.GetListingAsync(listingId);
        if (listing == null)
        {
            return NotFound($"Listing with ID {listingId} not found.");
        }

        return Ok(listing);
    }

    // POST: api/listing
    [HttpPost]
    [Route("listing/{userId}")]
    public async Task<IActionResult> CreateListing([FromBody] ListingDto listingDto, Guid userId)
    {
        if (listingDto == null)
        {
            return BadRequest("Listing object is null.");
        }

        try
        {
            var createdListing = await _listingService.CreateListing(userId, listingDto);
            return CreatedAtAction(nameof(GetListing), new { listingId = createdListing.ListingId }, createdListing);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the listing: {ex.Message}");
        }
    }

    // PUT: api/listing/{id}
    [HttpPut]
    [Route("{listingId}/")]
    public async Task<IActionResult> UpdateListing([FromRoute] Guid listingId, [FromBody] UpdateListingDto updateListingDto)
    {
        var updatedListing = await _listingService.UpdateListing(listingId, updateListingDto);
        if (updatedListing == null)
        {
            return NotFound($"Listing with ID {listingId} not found.");
        }

        return Ok(updatedListing);
    }

    // DELETE: api/listing/{id}
    [HttpDelete("{listingId}/")]
    public async Task<IActionResult> DeleteListing([FromRoute] Guid listingId)
    {
        var deletedListing = await _listingService.DeleteListing(listingId);
        if (deletedListing == null)
        {
            return NotFound($"Listing with ID {listingId} not found.");
        }

        return Ok(deletedListing);
    }
}

