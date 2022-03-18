using Warden.Data;
using Warden.Exceptions;
using Warden.Models;

namespace Warden.Services;

public class UserService
{
    private readonly WardenContext _context;

    public UserService(WardenContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}
