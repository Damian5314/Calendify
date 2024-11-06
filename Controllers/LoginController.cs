using System.Text; // Importeert mogelijk benodigde tekstfunctionaliteit (hoewel hier niet gebruikt)
using Microsoft.AspNetCore.Mvc; 
using StarterKit.Services; 

namespace StarterKit.Controllers
{
    [Route("api/v1/Login")] // Geeft aan dat alle routes in deze controller beginnen met "api/v1/Login"
    public class LoginController : Controller
    {
        // _loginService is een instantie van ILoginService die gebruikt wordt voor login- en registratieacties
        private readonly ILoginService _loginService;

        // Constructor van de LoginController die de ILoginService instantie instelt
        public LoginController(ILoginService loginService)
        {
            // De loginService wordt opgeslagen in het veld _loginService voor gebruik in andere methodes van deze controller
            _loginService = loginService;
        }

        // Login-methode die een POST-verzoek accepteert om een gebruiker in te loggen
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginBody loginBody)
        {
            // Controleert of het ingevoerde wachtwoord overeenkomt met de e-mail in de database
            var loginState = _loginService.CheckPassword(loginBody.Email!, loginBody.Password!);

            // Switch-structuur om verschillende inlogresultaten af te handelen
            switch (loginState)
            {
                case LoginStatus.Success: // Gebruiker is succesvol ingelogd
                    var userId = _loginService.GetUserIdByEmail(loginBody.Email); // Haalt het user ID op op basis van e-mail
                    HttpContext.Session.SetInt32("UserId", userId); // Slaat het gebruikers-ID op in de sessie
                    HttpContext.Session.SetString("AdminSession", "LoggedIn"); // Zet de admin-sessie-status op "LoggedIn"
                    HttpContext.Session.SetString("LoggedInAdmin", $"{loginBody.Email}"); // Slaat de e-mail op voor admin-controle
                    HttpContext.Session.SetString("LoggedInUser", $"{loginBody.Email}"); // Slaat de e-mail op voor algemene login-controle
                    return Ok($"Hello {loginBody.Email}, login success"); // Retourneert een succesbericht met 200 OK

                case LoginStatus.IncorrectPassword: // Foutieve wachtwoord
                    return Unauthorized("Incorrect password"); // Retourneert een 401 Unauthorized-fout

                case LoginStatus.IncorrectEmail: // Foutieve e-mail
                    return Unauthorized("Incorrect email"); // Retourneert een 401 Unauthorized-fout

                default: // Onverwachte fout
                    return BadRequest(); // Retourneert een 400 Bad Request-fout
            }
        }

        // Methode voor registratie die een POST-verzoek accepteert om een nieuwe gebruiker aan te maken
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterBody registerBody)
        {
            // Controleert of het request body null is
            if (registerBody == null)
            {
                return BadRequest("Request body is null."); // Retourneert een fout als het body leeg is
            }

            // Controleert of alle vereiste velden zijn ingevuld, anders geeft het een foutmelding
            if (string.IsNullOrEmpty(registerBody.FirstName))
            {
                return BadRequest("First name is required.");
            }

            if (string.IsNullOrEmpty(registerBody.LastName))
            {
                return BadRequest("Last name is required.");
            }

            if (string.IsNullOrEmpty(registerBody.Email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrEmpty(registerBody.Password))
            {
                return BadRequest("Password is required.");
            }

            if (string.IsNullOrEmpty(registerBody.RecuringDays))
            {
                return BadRequest("Recurring days are required.");
            }

            // Probeert de gebruiker te registreren met de opgegeven informatie
            var isRegistered = _loginService.RegisterUser(
                registerBody.FirstName,
                registerBody.LastName,
                registerBody.Email,
                registerBody.Password,
                registerBody.RecuringDays
            );

            // Controleert of registratie succesvol is
            if (isRegistered)
            {
                return Ok("User registration successful."); // Retourneert een succesbericht als registratie geslaagd is
            }

            return BadRequest("User already exists."); // Geeft een fout als de gebruiker al bestaat
        }

        // Controleert of een admin is ingelogd
        [HttpGet("IsAdminLoggedIn")]
        public IActionResult IsAdminLoggedIn()
        {
            // Controleert of de sessie geregistreerd is als "LoggedIn"
            if (IsSessionRegistered()) 
                return Ok($"Logged in as {HttpContext.Session.GetString("LoggedInAdmin")}"); // Geeft een succesbericht als de admin is ingelogd
            return Unauthorized("You are not logged in as Admin"); // Retourneert een fout als admin niet is ingelogd
        }

        // Controleert of een gebruiker is ingelogd
        [HttpGet("IsUserLoggedIn")]
        public IActionResult IsUserLoggedIn()
        {
            // Controleert of de sessie geregistreerd is als "LoggedIn"
            if (IsSessionRegistered()) 
                return Ok($"Logged in as {HttpContext.Session.GetString("LoggedInUser")}"); // Geeft een succesbericht als de gebruiker is ingelogd
            return Unauthorized("You are not logged in"); // Retourneert een fout als gebruiker niet is ingelogd
        }

        // Methode om te controleren of de sessie is geregistreerd als ingelogd
        [HttpGet("IsSessionRegistered")]
        public bool IsSessionRegistered()
        {
            // Controleert of de AdminSession-sessievariabele is ingesteld op "LoggedIn"
            return HttpContext.Session.GetString("AdminSession") == "LoggedIn";
        }

        // Methode om de gebruiker uit te loggen
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            // Controleert of de gebruiker is ingelogd
            if (IsSessionRegistered())
            {
                HttpContext.Session.SetString("AdminSession", "LoggedOut"); // Zet de sessiestatus op "LoggedOut"
                return Ok("Logged out"); // Retourneert een succesbericht dat de gebruiker is uitgelogd
            }
            return BadRequest("You are not logged in"); // Geeft een foutmelding als er niemand is ingelogd
        }
    }

    // Klasse die de login-informatie bevat, inclusief e-mail en wachtwoord
    public class LoginBody
    {
        public string? Email { get; set; } // E-mailadres van de gebruiker voor inloggen
        public string? Password { get; set; } // Wachtwoord van de gebruiker voor inloggen
    }

    // Klasse die de registratiegegevens van de gebruiker bevat
    public class RegisterBody
    {
        public string? FirstName { get; set; } // Voornaam van de gebruiker
        public string? LastName { get; set; } // Achternaam van de gebruiker
        public string? Email { get; set; } // E-mailadres van de gebruiker
        public string? Password { get; set; } // Wachtwoord van de gebruiker
        public string? RecuringDays { get; set; } // Herhalingsdagen voor de gebruiker
    }
}
