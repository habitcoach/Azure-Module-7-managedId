using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using mvcManagedIdKeyVaule.Models;
using System.Diagnostics;

namespace mvcManagedIdKeyVaule.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; 
        }

        public async Task <IActionResult> Index()
        {

            var keyVaultUri = _configuration["AzureKeyVault:VaultUrl"];
            var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential()); // System assigned

            //var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions // for user assigned
            //{ ManagedIdentityClientId = userAssignedClientId });


            try
            {
                var secretName = "Test";
                KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);
                var secretValue = secret.Value;

                // Use the secret value as needed in your application.
                ViewData["SecretValue"] = secretValue;
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}