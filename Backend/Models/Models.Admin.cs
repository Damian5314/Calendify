namespace StarterKit.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; } // Add this

        public string? RefreshToken { get; set; } // Add this 
        
    }
}
