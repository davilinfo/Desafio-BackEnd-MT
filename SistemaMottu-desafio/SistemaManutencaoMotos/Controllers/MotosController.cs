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
    /// Sistema de manutenção de motos
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("")]    
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class MotosController : ControllerBase
    {
        private readonly int _evtId = 21000;
        private readonly int _internalError = 500;        
        private readonly string _assemblyName = "SistemaManutencaoMotos";
        private readonly string _invalidRequest = "Dados inválidos";
        private readonly string _badFormatRequest = "Request mal formada";
        private readonly string _motoNotFound = "Moto não encontrada";
        private readonly string _motoresponse = "moto response";
        private readonly string _putOk = "Placa modificada com sucesso";
        private readonly string _registerMoto = "Post Register moto request";        
        private readonly string _getMotos = "Get motos request";
        private readonly string _getMotosId = "Get motos by id request";
        private readonly string _delMotosId = "Delete moto by id request";
        private readonly string _putMoto = "Put moto request";
        private readonly EventId _eventId;
        private readonly IConfiguration _config;
        private readonly IApplicationServiceMotocycleBike _applicationServiceMotocycleBike;
        private readonly ILogger<MotosController> _logger;

        public MotosController(IApplicationServiceMotocycleBike applicationServiceMotocycleBike, IConfiguration configuration, 
            ILogger<MotosController> logger)
        {
            _applicationServiceMotocycleBike = applicationServiceMotocycleBike;
            _config = configuration;
            _logger = logger;
            _eventId = new EventId(_evtId, _assemblyName);
        }
        
        /// <summary>
        /// Cadastrar uma nova moto
        /// </summary>
        /// <param name="request">identificador, ano, modelo, placa da moto</param>
        /// <returns></returns>
        [HttpPost("motos")]
        [ProducesResponseType<EmptyResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> Motos([FromBody] RequestMotocycleAdd request)
        {
            _logger.LogInformation(_eventId, _registerMoto);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceMotocycleBike.CreateAsync(request);
                    _logger.LogInformation(_eventId, null, $"{_motoresponse} {result}");
                    return new JsonResult(result) { ContentType = "application/json", StatusCode = 201 };
                }
                foreach(var item in ModelState.Values)
                {
                    foreach(var error in item.Errors)
                    {
                        _logger.LogError(_eventId, error.ErrorMessage);
                    }
                }
                var message = new ApplicationResponse($"{_invalidRequest}");                
                return new JsonResult (new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
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
        /// Consultar motos existentes
        /// </summary>
        /// <param name="placa">placa</param>
        /// <returns>Lista de motos</returns>
        [HttpGet("motos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ResponseMotocycleBike>))]        
        public async Task<IActionResult> GetMotos([FromQuery] RequestMotocyclePlate plate)
        {
            _logger.LogInformation(_eventId, _getMotos);
            try
            {
                if (string.IsNullOrEmpty(plate.placa))
                {
                    var result = await _applicationServiceMotocycleBike.GetAllAsync();
                    _logger.LogInformation(_eventId, null, $"{_motoresponse} {result}");
                    return Ok(result);
                }
                else
                {
                    var result = await _applicationServiceMotocycleBike.GetByPlate(plate.placa);                    
                    _logger.LogInformation(_eventId, null, $"{_motoresponse} {result}");
                    return Ok(result);
                }                
            }
            catch(Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new StatusCodeResult(_internalError);
            }
        }

        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        /// <param name="id">identificador moto</param>
        /// <param name="request">placa</param>
        /// <returns></returns>
        [HttpPut("motos/{id}/placa")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationResponse))]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> Put(string id, [FromBody]RequestMotocycleUpdate request)
        {
            _logger.LogInformation(_eventId, _putMoto);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceMotocycleBike.UpdateAsync(id, request);
                    _logger.LogInformation(_eventId, null, $"{_motoresponse} {result}");

                    var messageOk = new ApplicationResponse(_putOk);
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
                return BadRequest(message.Message);
            }
            catch(BusinessException be)
            {
                _logger.LogError(_eventId, be, be.Message);
                var message = new ApplicationResponse($"{be.Message}");
                return new JsonResult(new Dictionary<string, string> { { "mensagem", message.Message } }) { ContentType = "application/json", StatusCode = 400 };
            }
            catch(Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new StatusCodeResult(_internalError);
            }            
        }

        /// <summary>
        /// Consultar motos existes por id
        /// </summary>
        /// <param name="request">id</param>
        /// <returns></returns>
        [HttpGet("motos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMotocycleBike))]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> GetMotosById(string id)
        {
            _logger.LogInformation(_eventId, _getMotosId);
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _applicationServiceMotocycleBike.GetById(id);
                    if (result == null)
                    {
                        return NotFound(_motoNotFound);
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
                var message = new ApplicationResponse($"{_badFormatRequest}");
                return BadRequest(message);
            }catch(Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new StatusCodeResult(_internalError);
            }
        }

        [HttpDelete("motos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResponse))]
        [ProducesResponseType<ApplicationResponse>(StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> Remove(string id)
        {
            _logger.LogInformation(_eventId, _delMotosId);
            try
            {
                if (ModelState.IsValid)
                {
                    await _applicationServiceMotocycleBike.DeleteAsync(id);                    
                    return Ok();
                }
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        _logger.LogError(_eventId, error.ErrorMessage);
                    }
                }
                var message = new ApplicationResponse($"{_invalidRequest}");
                return BadRequest(message.Message);
            }
            catch(BusinessException be)
            {
                _logger.LogError(_eventId, be, be.Message);
                var message = new ApplicationResponse($"{_invalidRequest} {be.Message}");
                return BadRequest(message.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(_eventId, e, e.Message);
                return new StatusCodeResult(_internalError);
            }
        }
    }
}
