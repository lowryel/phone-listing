using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilePhoneListing.DBContext;
using PhoneListingAPI.PhoneListingDto;
using PhoneListingAPI.Services;

namespace PhoneListingAPI.Controllers;

[ApiController]
[Route("api/")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly MobilePhoneDbContext _mobilePhoneDbContext;
    public UserController(MobilePhoneDbContext mobilePhoneDbContext, IUserService userService)
    {
        _mobilePhoneDbContext=mobilePhoneDbContext;
        _userService = userService;
    }


    [HttpPost]
    [Route("user/")]
    public async Task<ActionResult<User>> CreateUser(User newuser)
    {
        if (newuser == null)
        {
            return BadRequest("User data is null.");
        }

        if (string.IsNullOrEmpty(newuser.Username))
        {
            return BadRequest("Username is required.");
        }

        var existingUser = await _mobilePhoneDbContext.Users
            .AnyAsync(u => u.Username == newuser.Username);
        if (existingUser)
        {
            return Conflict("Username already exists.");
        }
        var user = await _userService.CreateNewUser(newuser);

        // Optionally check for uniqueness before adding
        return CreatedAtAction(nameof(CreateUser), new { userId = user.UserId }, user);
    }


    [HttpGet]
    [Route("users/{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        
        if (user == null)
        {
            return NotFound("No users available");
        }

        return Ok(user);
    }


    [HttpGet]
    [Route("users/active")]
    public async Task<ActionResult<UserDto>> GetAllActiveUsers()
    {
        var user = await _userService.GetAllActiveUsers();
        if (user == null)
        {
            return NotFound("No active users available");
        }
        return Ok(user);
    }



    [HttpGet]
    [Route("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var userDtos = await _userService.GetAllUsers();
        
        if (userDtos == null)
        {
            return NotFound("No users available");
        }
        return Ok(userDtos);
    }

    [HttpPut]
    [Route("user/{userId}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> UpdateUser(Guid userId, UpdateUserDto updateUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var updatedUser = await _userService.UpdateUser(userId, updateUser);
        
        if (updatedUser == null)
        {
            return NotFound($"User with ID {userId} not found");
        }
        return Ok(updatedUser);
    }

    [HttpDelete]
    [Route("user/{userId}")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _userService.Delete(userId);
        return NoContent();
    }
}
