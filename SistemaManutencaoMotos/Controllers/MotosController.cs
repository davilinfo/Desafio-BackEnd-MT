using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace SistemaManutencaoMotos.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class MotosController : ControllerBase
    {        

        private readonly ILogger<MotosController> _logger;

        public MotosController(ILogger<MotosController> logger)
        {
            _logger = logger;
        }        
    }
}
