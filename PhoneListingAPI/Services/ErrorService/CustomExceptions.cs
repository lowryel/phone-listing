using System;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace PhoneListingAPI.Services.ErrorService;

public class UserCreationException : Exception, ICustomExceptions
{
    public UserCreationException(string message) : base(message)
    {
    }

    public void ErrorInfo(string message)
    {
        
    }
}

public interface ICustomExceptions
{
    void ErrorInfo(string message);
}
