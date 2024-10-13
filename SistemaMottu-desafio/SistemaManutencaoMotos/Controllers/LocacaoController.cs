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
    /// 
    /// </summary>
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class LocacaoController : ControllerBase
    {
        private readonly int _evtId = 21000;
        private readonly int _internalError = 500;
        private readonly string _assemblyName = "SistemaManutencaoMotos";
        private readonly string _invalidRequest = "Dados inválidos";
        private readonly string _registerLease = "Post Register lease request";
        private readonly string _updateLease = "Post update lease devolution request";
        private readonly string _getLeaseById = "Get lease by id request";
        private readonly string _leaseNotFound = "Locação não encontrada";
        private readonly string _leaseUpdateOk = "Data de devolução informada com sucesso";
        private readonly string _leaseresponse = "lease response";
        private readonly EventId _eventId;
        private readonly IConfiguration _config;
        private readonly IApplicationServiceLease _applicationServiceLease;
        private readonly ILogger<LocacaoController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServiceLease"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public LocacaoController(IApplicationServiceLease applicationServiceLease, IConfiguration configuration,
            ILogger<LocacaoController> logger)
        {
            _applicationServiceLease = applicationServiceLease;
            _logger = logger;
            _config = configuration;
            _eventId = new EventId(_evtId, _assemblyName);
        }

        /// <summary>
        /// Alugar uma moto
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("locacao")]
        [ProducesResponseType<EmptyResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Lease([FromBody] RequestLeaseAdd request)
        {
            _logger.LogInformation(_eventId, _registerLease);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceLease.CreateAsync(request);
                    _logger.LogInformation(_eventId, null, $"{_leaseresponse} {result}");
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
                var message = new ApplicationResponse($"{be.Message}");
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
        /// Consultar locação por id
        /// </summary>
        /// <param name="id">identificador locação</param>
        /// <returns>Locação</returns>
        [HttpGet("locacao/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMotocycleBike))]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLeaseById(string id)
        {
            _logger.LogInformation(_eventId, _getLeaseById);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceLease.GetById(id);
                    if (result == null)
                    {
                        return NotFound(_leaseNotFound);
                    }
                    return Ok(result);
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
            catch (Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new JsonResult(new Dictionary<string, string> { { "mensagem", e.Message } }) { ContentType = "application/json", StatusCode = 500 };
            }
        }

        /// <summary>
        /// Informar data devolução e calcular valor
        /// </summary>
        /// <param name="id">identificador locação</param>
        /// <param name="request">data devolução</param>
        /// <returns>mensagem de texto</returns>
        [HttpPut("locacao/{id}/devolucao")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationResponse))]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(string id, [FromBody] RequestLeaseUpdate request)
        {
            _logger.LogInformation(_eventId, _updateLease);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceLease.UpdateAsync(id, request);
                    _logger.LogInformation(_eventId, null, $"{_leaseresponse} {result}");

                    var messageOk = new ApplicationResponse(_leaseUpdateOk);
                    return Ok(messageOk);
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
                var message = new ApplicationResponse($"{be.Message}");
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
