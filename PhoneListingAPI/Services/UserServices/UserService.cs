using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using Microsoft.VisualBasic;
using MobilePhoneListing.DBContext;
using PhoneListingAPI.PhoneListingDto;
// using PhoneListingAPI.PhoneListingDto.ListingDto;
using PhoneListingAPI.Services.ErrorService;

namespace PhoneListingAPI.Services;

public class UserService : IUserService
{
    private readonly MobilePhoneDbContext _mobilePhoneDbContext;
    private readonly ILogger<UserService> _logger;

    public UserService(MobilePhoneDbContext mobilePhoneDbContext, ILogger<UserService> logger)
    {
        _mobilePhoneDbContext = mobilePhoneDbContext;
        _logger = logger;
    }


    public async Task<User> CreateNewUser([FromBody] User user)
    {
        try
        {
            // Try to add the user to the database
            user.SetPassword(user.PasswordHash);
            await _mobilePhoneDbContext.Users.AddAsync(user);

            // Save the changes to the database
            await _mobilePhoneDbContext.SaveChangesAsync();
            _logger.LogInformation("user creation successful");
            _logger.LogDebug("user creation successful");

            // Return the user after successful creation
            return user;
        }
        catch (DbUpdateException dbEx)
        {
            // Log the specific database exception
            _logger.LogError($"Database error occurred while creating user: {dbEx.Message}", dbEx);

            // Optionally rethrow or return a specific error response (depending on your service pattern)
            throw new UserCreationException("An error occurred while saving the user to the database. Please try again.");
        }
        catch (Exception ex)
        {
            // Log any other exceptions that might occur
            _logger.LogError($"An unexpected error occurred: {ex.Message}", ex);

            // Rethrow the exception or handle it accordingly
            throw new Exception("An unexpected error occurred while creating the user.");
        }
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _mobilePhoneDbContext.Users.Include(u => u.Listings).ToListAsync();
        
        if (users == null || users.Count == 0)
        {
            _logger.LogError($"No user data found {users}");
            return null!;
        }

        _logger.LogInformation("User Information");
        return users.Select(u => new UserDto
        {
            Email = u.Email,
            Username = u.Username,
            Id = u.UserId,
            Listings = u.Listings
        });
    }


    public async Task<IEnumerable<UserDto>> GetAllActiveUsers()
    {
        // Fetch users where IsDeleted is false
        var users = await _mobilePhoneDbContext.Users
                            .Include(u => u.Listings)
                            .Where(u => u.IsDeleted == false) // Filter for non-deleted users
                            .ToListAsync();
        
        if (users == null || users.Count == 0)
        {
            _logger.LogError("No active user data found");
            return null!;
        }

        _logger.LogInformation("User Information retrieved successfully");
        
        // Return filtered user data as UserDto
        return users.Select(u => new UserDto
        {
            Email = u.Email,
            Username = u.Username,
            Id = u.UserId,
            Listings = u.Listings
        });
    }

    public async Task<UserDto> GetUserAsync([FromRoute]Guid userId)
    {
        
        var user = await _mobilePhoneDbContext.Users.Include(u => u.Listings).FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null )
        {
            return null!;
        }

        var os = Environment.OSVersion;
        var env = Environment.GetEnvironmentVariable("DB_STRING");
        var ID = await Task.Run(() => Half.NegativeZero);
        
        _logger.LogInformation($"User {ID}");
        _logger.LogInformation($"Environmental Variable: {env}");

        _logger.LogInformation($"user information: {user.UserId}");
        return new UserDto
        {
            Id = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Listings = user.Listings
        };
    }

    public async Task<UserDto> UpdateUser([FromRoute]Guid userId, [FromBody]UpdateUserDto updateUser)
    {
        var user = await _mobilePhoneDbContext.Users.FindAsync(userId);
        if (user == null)
        {
            return null!;
        }

        // Update only the properties that are not null in the DTO
        if (updateUser.Username != null)
        {
            user.Username = updateUser.Username;
        }
        if (updateUser.Email != null)
        {
            user.Email = updateUser.Email;
        }

        _mobilePhoneDbContext.Users.Update(user);
        await _mobilePhoneDbContext.SaveChangesAsync();

        return new UserDto
        {
            Id = user.UserId,
            Username = user.Username,
            Email = user.Email
        };

    }

    public async Task<User> Delete(Guid userId)
    {
        var user = await _mobilePhoneDbContext.Users.FindAsync(userId);
        if (user == null) 
            return null!;
        _mobilePhoneDbContext.Users.Remove(user);
        await _mobilePhoneDbContext.SaveChangesAsync();
        return user;
    }
}

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsers();

    Task<User> CreateNewUser([FromBody] User user);

    Task<UserDto> GetUserAsync([FromRoute]Guid userId);

    Task<UserDto> UpdateUser([FromRoute]Guid userId, [FromBody]UpdateUserDto updateUuser);

    Task<User> Delete(Guid userId);

    Task<IEnumerable<UserDto>> GetAllActiveUsers();
}



