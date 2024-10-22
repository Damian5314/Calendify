using StarterKit.Models;

namespace StarterKit.Services
{
    public enum LoginStatus { IncorrectPassword, IncorrectEmail, Success}

    public interface ILoginService
    {
        LoginStatus CheckPassword(string email, string inputPassword);
        bool RegisterUser(string firstName, string lastName, string email, string password, string recuringDays);
    }
}
