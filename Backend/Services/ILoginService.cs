using StarterKit.Models;

namespace StarterKit.Services
{
    public interface ILoginService
    {
        // Method to perform password check
        (LoginStatus status, string role) CheckPassword(string email, string password);
        
        // Method to get user ID by email
        int GetUserIdByEmail(string email);

        int GetAdminIdByEmail(string email);
        
        string GetFirstNameByEmail(string email);

        string GetUserNameByEmailAdmin(string email);
        
        // Method to register a new user
        bool RegisterUser(string firstName, string lastName, string email, string password, string recuringDays);
        
        // Admin methods
        bool RegisterAdmin(string userName, string email, string password);
        (bool isValid, string role) AdminLogin(string email, string password);

        // Password reset functionality
        bool GeneratePasswordResetToken(string email); // Generates a token and sends it via email
        bool ResetPassword(string token, string newPassword); // Validates the token and resets the password
        
        // Remove or correct this method as it's not implemented in LoginService
        // object ValidateUser(string email, string password);
    }
}