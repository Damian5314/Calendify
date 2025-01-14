using System.Diagnostics; // Importeert functionaliteit voor diagnostische informatie, zoals het vastleggen van een unieke Request ID
using Microsoft.AspNetCore.Mvc; // Importeert basisfunctionaliteit voor het werken met MVC-controllers en het sturen van responses
using StarterKit.Models; // Importeert de modellen, waaronder het ErrorViewModel dat gebruikt wordt om foutinformatie te tonen

namespace StarterKit.Controllers
{
    // Deze controller handelt verzoeken af die naar de Home-sectie van de applicatie gaan
    public class HomeController : Controller
    {
        // _logger is een veld dat de logger voor deze controller opslaat
        // ILogger<HomeController> is specifiek voor deze controller, wat helpt bij het organiseren en filteren van logberichten
        private readonly ILogger<HomeController> _logger;

        // Constructor van de HomeController, die de ILogger injecteert
        public HomeController(ILogger<HomeController> logger)
        {
            // De meegegeven logger wordt opgeslagen in het veld _logger
            // Hiermee kunnen we logberichten schrijven in de hele controller
            _logger = logger;
        }

        // Methode die GET-verzoeken afhandelt voor de hoofdpagina of andere routes binnen de controller
        [HttpGet("{**slug}")] // Deze annotatie vangt alle URL's op na de basisroute van de controller
        public IActionResult Index()
        {
            // Retourneert de standaardview voor de hoofdpagina (meestal "Index.cshtml" in Views/Home/)
            return View();
        }

        // Methode die een foutpagina toont bij fouten in de applicatie
        // De ResponseCache-attribuut voorkomt dat deze pagina wordt opgeslagen in de cache
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Maakt een nieuw ErrorViewModel-object aan en vult het RequestId-veld in
            // Het RequestId helpt om dit specifieke verzoek te traceren in de logs
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
