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

        public LoginStatus CheckPassword(string email, string inputPassword)
        {
            var admin = _context.Admin.FirstOrDefault(x => x.Email == email);
            if (admin == null)
            {
                return LoginStatus.IncorrectEmail;
            }

            return admin.Password == inputPassword
                ? LoginStatus.Success
                : LoginStatus.IncorrectPassword;
        }

        public bool RegisterUser(string firstName, string lastName, string email, string password, string recuringDays)
        {
            if (_context.User.Any(u => u.Email == email))
            {
                return false;
            }

            var newUser = new User
            {   
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                RecuringDays = recuringDays,
                Attendances = new List<Attendance>(),
                Event_Attendances = new List<Event_Attendance>(),
            };

            _context.User.Add(newUser);
            _context.SaveChanges();
            return true;
        }
    }
}
