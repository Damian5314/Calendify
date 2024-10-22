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

        public LoginStatus CheckPassword(string username, string inputPassword)
        {
            var admin = _context.Admin.FirstOrDefault(x => x.UserName == username);
            if (admin == null)
            {
                return LoginStatus.IncorrectUsername;
            }

            return admin.Password == inputPassword
                ? LoginStatus.Success
                : LoginStatus.IncorrectPassword;
        }
    }
}
