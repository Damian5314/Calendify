using StarterKit.Models;

namespace StarterKit.Services
{
    public interface ILoginService
    {
        // Methode om wachtwoordcontrole uit te voeren
        LoginStatus CheckPassword(string email, string password);
        
        // Methode om de gebruikers-ID op te halen via e-mail
        int GetUserIdByEmail(string email);

        int GetAdminIdByEmail(string email);
        
        string GetFirstNameByEmail(string email);

        string GetUserNameByEmailAdmin(string email);
        
        // Methode om een nieuwe gebruiker te registreren
        bool RegisterUser(string firstName, string lastName, string email, string password, string recuringDays);
        
        // Methodes voor admins
        bool RegisterAdmin(string userName, string email, string password);
        (bool isValid, string role) AdminLogin(string email, string password);

        // Wachtwoord vergeten-functionaliteit
        bool GeneratePasswordResetToken(string email); // Genereert een token en verstuurt het via e-mail
        bool ResetPassword(string token, string newPassword); // Valideert de token en reset het wachtwoord
    }
}
