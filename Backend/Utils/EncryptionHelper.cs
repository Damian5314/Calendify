using System.Security.Cryptography;
using System.Text;

namespace StarterKit.Utils
{
    public static class EncryptionHelper
    {
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes); // Convert hash to Base64 string
            }
        }
    }
}