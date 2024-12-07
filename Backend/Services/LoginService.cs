using StarterKit.Models;
using StarterKit.Utils;

namespace StarterKit.Services
{
    public class LoginService : ILoginService
    {
        private readonly DatabaseContext _context;

        public LoginService(DatabaseContext context)
        {
            _context = context;
        }

        // Check Password for both User and Admin
        public LoginStatus CheckPassword(string email, string password)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.Email == email);
            if (admin != null)
            {
                return admin.Password == password ? LoginStatus.Success : LoginStatus.IncorrectPassword;
            }

            var user = _context.User.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return user.Password == password ? LoginStatus.Success : LoginStatus.IncorrectPassword;
            }

            return LoginStatus.IncorrectEmail;
        }

        // Register User
        public bool RegisterUser(string firstName, string lastName, string email, string password, string recuringDays)
        {
            if (_context.User.Any(u => u.Email == email))
                return false;

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Role = "User",
                RecuringDays = recuringDays
            };

            _context.User.Add(user);
            _context.SaveChanges();
            return true;
        }

        // Register Admin
        public bool RegisterAdmin(string userName, string email, string password)
        {
            // Check if the username already exists
            if (_context.Admin.Any(a => a.UserName == userName))
            {
                throw new InvalidOperationException("Admin username already exists.");
            }

            // Check if the email already exists
            if (_context.Admin.Any(a => a.Email == email))
            {
                throw new InvalidOperationException("Admin email already exists.");
            }

            var newAdmin = new Admin
            {
                UserName = userName,
                Email = email,
                Password = EncryptionHelper.EncryptPassword(password) // Always hash passwords
            };

            _context.Admin.Add(newAdmin);
            _context.SaveChanges();
            return true;
        }


        // Admin Login
        public (bool isValid, string role) AdminLogin(string email, string password)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (admin != null)
            {
                return (true, "Admin");
            }
            return (false, string.Empty);
        }

        // Get User ID by Email
        public int GetUserIdByEmail(string email)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);
            return user?.UserId ?? 0;
        }
    }
}
