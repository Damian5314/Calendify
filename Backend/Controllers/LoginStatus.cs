namespace StarterKit.Models
{
    public enum LoginStatus
    {
        Success,          // Login succeeded
        IncorrectEmail,   // Email not found in the database
        IncorrectPassword // Email found, but password mismatch
    }
}
