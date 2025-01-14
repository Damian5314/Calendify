using StarterKit.Models;

namespace StarterKit.Services
{
    public interface ILoginService
    {
        LoginStatus CheckPassword(string email, string password);
        int GetUserIdByEmail(string email);
        bool RegisterUser(string firstName, string lastName, string email, string password, string recuringDays);
        
        // Add these for admins
        bool RegisterAdmin(string userName, string email, string password);
        (bool isValid, string role) AdminLogin(string email, string password);
    }
}
