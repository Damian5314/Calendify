using StarterKit.Models;

namespace StarterKit.Services
{
    public enum LoginStatus { IncorrectPassword, IncorrectUsername, Success }

    public interface ILoginService
    {
        LoginStatus CheckPassword(string username, string inputPassword);
    }
}
