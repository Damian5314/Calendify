namespace StarterKit.Services;

using StarterKit.Models;
using StarterKit.Utils;



public enum LoginStatus { IncorrectPassword, IncorrectUsername, Success }

public enum ADMIN_SESSION_KEY { adminLoggedIn }

public class LoginService : ILoginService
{

    private readonly DatabaseContext _context;

    public LoginService(DatabaseContext context)
    {
        _context = context;
    }

    public LoginStatus CheckPassword(string username, string inputPassword)
    {
        // TODO: Make this method check the password with what is in the database
        if (_context.Admin.Any(x => x.UserName == username && x.Password == inputPassword))
        {
            return LoginStatus.Success;
        }
        else if (_context.Admin.Any(x => x.UserName == username && x.Password != inputPassword))
        {
            return LoginStatus.IncorrectPassword;
        }
        return LoginStatus.IncorrectUsername;
    }
}