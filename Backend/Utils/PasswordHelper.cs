namespace StarterKit.Utils
{
    public static class PasswordHelper
    {
        public static bool ValidatePassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsUpper);
        }
    }
}
