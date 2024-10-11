using Application.Exception;
using Application.Interface;
using Application.Models.Request;
using Application.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace SistemaManutencaoMotos.Controllers
{
    /// <summary>
    /// Sistema de manutenção de entregador
    /// </summary>    
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class EntregadoresController : ControllerBase
    {
        private readonly int _evtId = 21000;
        private readonly int _internalError = 500;
        private readonly string _assemblyName = "SistemaManutencaoMotos";
        private readonly string _invalidRequest = "Dados inválidos";        
        private readonly string _registerDeliver = "Post Register deliver request";        
        private readonly string _updateDeliver = "Post update deliver cnh request";
        private readonly string _deliverresponse = "deliver response";
        private readonly EventId _eventId;
        private readonly IConfiguration _config;
        private readonly IApplicationServiceDeliver _applicationServiceDeliver;
        private readonly ILogger<EntregadoresController> _logger;

        public EntregadoresController(IApplicationServiceDeliver applicationServiceDeliver, IConfiguration configuration,
            ILogger<EntregadoresController> logger)
        {
            _applicationServiceDeliver = applicationServiceDeliver;
            _logger = logger;
            _config = configuration;
            _eventId = new EventId(_evtId, _assemblyName);
        }

        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("entregadores")]
        [ProducesResponseType<EmptyResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delivers([FromBody] RequestDeliverAdd request)
        {
            _logger.LogInformation(_eventId, _registerDeliver);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceDeliver.CreateAsync(request);
                    _logger.LogInformation(_eventId, null, $"{_deliverresponse} {result}");
                    return Created("",result);
                }
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        _logger.LogError(_eventId, error.ErrorMessage);
                    }
                }
                var message = new ApplicationResponse($"{_invalidRequest}");
                return new JsonResult(new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
            }
            catch (BusinessException be)
            {
                _logger.LogError(_eventId, be, be.Message);
                var message = new ApplicationResponse($"{_invalidRequest} {be.Message}");
                return new JsonResult(new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError(_eventId, ioe, ioe.Message);
                var message = new ApplicationResponse($"{_invalidRequest} {ioe.Message}");
                return new JsonResult(new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
            }
            catch (Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new JsonResult(new Dictionary<string, string> { { "mensagem", e.Message } }) { ContentType = "application/json", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Enviar foto da CNH
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("entregadores/{id}/cnh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationResponse))]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDeliver(string id, [FromBody] RequestDeliverUpdate request)
        {
            _logger.LogInformation(_eventId, _updateDeliver);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceDeliver.UpdateAsync(id, request);
                    _logger.LogInformation(_eventId, null, $"{_deliverresponse} {result}");
                    
                    return new JsonResult(result) { ContentType = "application/json", StatusCode = 201 };
                }
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        _logger.LogError(_eventId, error.ErrorMessage);
                    }
                }
                var message = new ApplicationResponse($"{_invalidRequest}");
                return new JsonResult(new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
            }
            catch (BusinessException be)
            {
                _logger.LogError(_eventId, be, be.Message);
                var message = new ApplicationResponse($"{_invalidRequest} {be.Message}");
                return new JsonResult(new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
            }
            catch (Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new JsonResult(new Dictionary<string, string> { { "mensagem", e.Message } }) { ContentType = "application/json", StatusCode = 500 };
            }            
        }
    }
}
