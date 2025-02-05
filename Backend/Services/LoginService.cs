 using StarterKit.Models;
using System;
using StarterKit.Utils;
using System.Text.Json;

namespace StarterKit.Services
{
    public class LoginService : ILoginService
    {
        private readonly DatabaseContext _context;

        public LoginService(DatabaseContext context)
        {
            _context = context;
        }

        // 🔹 Haal terugkerende dagen op via userId
        public List<string> GetRecurringDays(int userId)
        {
            Console.WriteLine($"[GetRecurringDays] Fetching recurring days for user ID: {userId}");

            var user = _context.User.FirstOrDefault(u => u.UserId == userId);
            return user?.RecuringDays ?? new List<string>();
        }

        // 🔹 Update terugkerende dagen via userId
        public bool UpdateRecurringDays(int userId, List<string> newDays)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.UserId == userId);
                if (user == null) return false;

                user.RecuringDaysJson = JsonSerializer.Serialize(newDays); // ✅ Zet om naar JSON
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateRecurringDays] Error: {ex.Message}");
                return false;
            }
        }


        // 🔹 Update e-mail
       public bool UpdateEmail(int userId, string newEmail)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.UserId == userId);
                if (user == null) return false;

                if (_context.User.Any(u => u.Email == newEmail && u.UserId != userId))
                {
                    Console.WriteLine($"[UpdateEmail] Email '{newEmail}' already in use.");
                    return false;
                }

                user.Email = newEmail;
                _context.SaveChanges();

                // ✅ Controleer of het correct is opgeslagen
                return _context.User.Any(u => u.UserId == userId && u.Email == newEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateEmail] Error: {ex.Message}");
                return false;
            }
        }


        // 🔹 Update wachtwoord met validatie
        public bool UpdatePassword(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.UserId == userId);
                if (user == null) return false;

                if (user.Password != EncryptionHelper.EncryptPassword(currentPassword))
                {
                    Console.WriteLine("[UpdatePassword] Current password is incorrect.");
                    return false;
                }

                // ✅ Sla het geëncrypteerde wachtwoord op
                user.Password = EncryptionHelper.EncryptPassword(newPassword);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdatePassword] Error: {ex.Message}");
                return false;
            }
        }


        // 🔹 Register User with multiple recurring days
        public bool RegisterUser(string firstName, string lastName, string email, string password, List<string> recuringDays)
        {
            try
            {
                Console.WriteLine($"[RegisterUser] Attempting to register user: {email}");

                if (_context.User.Any(u => u.Email == email))
                {
                    Console.WriteLine($"[RegisterUser] Email '{email}' already exists.");
                    return false;
                }

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

                Console.WriteLine($"[RegisterUser] User '{email}' registered successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RegisterUser] Error: {ex.Message}");
                return false;
            }
        }

        // 🔹 Overige login- en wachtwoordfuncties blijven ongewijzigd
        public LoginStatus CheckPassword(string email, string password)
        {
            Console.WriteLine($"[CheckPassword] Validating user with email: {email}");

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

            Console.WriteLine($"[CheckPassword] Email '{email}' not found.");
            return LoginStatus.IncorrectEmail;
        }

        public bool RegisterAdmin(string userName, string email, string password)
        {
            try
            {
                if (_context.Admin.Any(a => a.UserName == userName) || _context.Admin.Any(a => a.Email == email))
                {
                    throw new InvalidOperationException("Admin username or email already exists.");
                }

                var newAdmin = new Admin
                {
                    UserName = userName,
                    Email = email,
                    Password = password
                };

                _context.Admin.Add(newAdmin);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public (bool isValid, string role) AdminLogin(string email, string password)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.Email == email && a.Password == password);
            return admin != null ? (true, "Admin") : (false, string.Empty);
        }

        public int GetUserIdByEmail(string email)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);
            return user?.UserId ?? 0;
        }

        public int GetAdminIdByEmail(string email)
        {
            var user = _context.Admin.FirstOrDefault(u => u.Email == email);
            return user?.AdminId ?? 0;
        }

        public string GetFirstNameByEmail(string email)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);
            return user?.FirstName ?? "null";
        }

        public string GetUserNameByEmailAdmin(string email)
        {
            var user = _context.Admin.FirstOrDefault(u => u.Email == email);
            return user?.UserName ?? "null";
        }

        public bool GeneratePasswordResetToken(string email)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.Email == email);
                if (user == null) return false;

                user.PasswordResetToken = Guid.NewGuid().ToString();
                user.TokenExpiry = DateTime.UtcNow.AddMinutes(30);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ResetPassword(string token, string newPassword)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.PasswordResetToken == token && u.TokenExpiry > DateTime.UtcNow);
                if (user == null) return false;

                user.Password = newPassword;
                user.PasswordResetToken = null;
                user.TokenExpiry = null;
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateResetToken(string token)
        {
            return _context.User.Any(u => u.PasswordResetToken == token && u.TokenExpiry > DateTime.UtcNow);
        }
    }
}