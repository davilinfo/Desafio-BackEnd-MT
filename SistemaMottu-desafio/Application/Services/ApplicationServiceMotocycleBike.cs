using Application.Exception;
using Application.Interface;
using Application.Models.Request;
using Application.Models.Response;
using AutoMapper;
using Domain.Contract;
using Domain.Entities;
using Domain.MotocycleBike.Commands;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class ApplicationServiceMotocycleBike : IApplicationServiceMotocycleBike, IBusinessValidation<MotocycleBike>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryMotocycleBike _repositoryMotocycleBike;
        private readonly IRepositoryLease _repositoryLease;
        private readonly INotify<ResponseMotocycleBike> _notify;
        private readonly IRedisCacheService _redisCacheService;
        private readonly string _plateFound = "Placa existente";
        private readonly string _invalid = "Dados inválidos";
        private readonly string _notSaved = "Dados não gravados";
        private readonly string _existMotoLease = "Existe registro de locação de moto";
        private readonly int _notAffectedSet = 0;
        public ApplicationServiceMotocycleBike(IMapper mapper, 
            IRepositoryMotocycleBike repositoryMotocycleBike, 
            IRepositoryLease repositoryLease,
            IRedisCacheService redisCacheService,
            INotify<ResponseMotocycleBike> notify) 
        { 
            _mapper = mapper;
            _repositoryMotocycleBike = repositoryMotocycleBike;
            _repositoryLease = repositoryLease;
            _redisCacheService = redisCacheService;
            _notify = notify;
        }

        public async Task<ResponseMotocycleBike> CreateAsync(RequestMotocycleAdd request)
        {
            var command = _mapper.Map<RegisterMotocycleBikeCommand>(request);            
            if (command.IsValid())
            {
                var entity = command.CreateMotocycleBike();
                RegisterBusinessValidation(entity);
                
                var result = await _repositoryMotocycleBike.Add(entity);
                var response = _mapper.Map<ResponseMotocycleBike>(entity);
                if (!string.IsNullOrEmpty(response.Identifier))
                {
                    _redisCacheService.InvalidateCacheEntry("ApplicationServiceMotocycleBike.getAllmotos");
                    _notify.NotifyMessage(response);
                    return response;
                }
                throw new InvalidOperationException(_notSaved);
            }
            var exceptionList = new StringBuilder();
            exceptionList.AppendLine(_invalid);
            command.ValidationResult.Errors.ForEach(command => { exceptionList.AppendLine(command.ErrorMessage); });
            
            throw new BusinessException(exceptionList.ToString());
        }

        public async Task DeleteAsync(string identifier)
        {
            var anyMoto = _repositoryLease.GetAll().Any(p=> p != null && p.MotocycleBikeId == identifier);
            if (anyMoto)
            {
                throw new BusinessException(_existMotoLease);
            }
            var result = await _repositoryMotocycleBike.Delete(identifier);
            
            if (result == _notAffectedSet)
            {
                throw new BusinessException(_invalid);
            }
            _redisCacheService.InvalidateCacheEntry("ApplicationServiceMotocycleBike.getAllmotos");
            _redisCacheService.InvalidateCacheEntry($"ApplicationServiceMotocycleBike.getById.{identifier}");
        }

        public Task<List<ResponseMotocycleBike>> GetAllAsync()
        {
            var inCacheStore = _redisCacheService.GetValue($"ApplicationServiceMotocycleBike.getAllmotos");
            if (string.IsNullOrEmpty(inCacheStore))
            {
                var result = (from item in _repositoryMotocycleBike.GetAll()
                             select new ResponseMotocycleBike
                             {
                                 Identifier = item.Identifier,
                                 Model = item.Model,
                                 Plate = item.Plate,
                                 Year = item.Year
                             }).ToList();
                var serialized = JsonSerializer.Serialize(result);
                _redisCacheService.SetValue("ApplicationServiceMotocycleBike.getAllmotos", serialized);
                return Task.FromResult(result);
            }
            else
            {
                var result = JsonSerializer.Deserialize<List<ResponseMotocycleBike>>(inCacheStore);
#pragma warning disable CS8619 
                return Task.FromResult(result);
#pragma warning restore CS8619 
            }                        
        }

        public async Task<ResponseMotocycleBike> GetById(string identifier)
        {
            var inCacheStore = _redisCacheService.GetValue($"ApplicationServiceMotocycleBike.getById.{identifier}");
            if (string.IsNullOrEmpty(inCacheStore))
            {
                var entity = await _repositoryMotocycleBike.GetById(identifier);
                if (entity != null)
                {
                    var result = _mapper.Map<ResponseMotocycleBike>(entity);
                    var serialized = JsonSerializer.Serialize(result);
                    _redisCacheService.SetValue($"ApplicationServiceMotocycleBike.getById.{identifier}", serialized);
                    return result;
                }
            }
            else
            {
                var result = JsonSerializer.Deserialize<ResponseMotocycleBike>(inCacheStore);
#pragma warning disable CS8603
                return result;
            }
            return null;
#pragma warning restore CS8603 
        }                

        public void RegisterBusinessValidation(MotocycleBike entity)
        {
            var result = _repositoryMotocycleBike.GetAll().Any(p => p != null && p.Plate.ToLower() == entity.Plate.ToLower());
            if (result)
            {
                throw new BusinessException(_plateFound);
            }
        }

        public void UpdateBusinessValidation(MotocycleBike entity)
        {
            var result = _repositoryMotocycleBike.GetAll().Any(p => p != null && p.Plate.ToLower() == entity.Plate.ToLower());
            if (result)
            {
                throw new BusinessException(_plateFound);
            }
        }

        public async Task<ResponseMotocycleBike> UpdateAsync(string identifier, RequestMotocycleUpdate request)
        {
            var entity = await _repositoryMotocycleBike.GetById(identifier);
            var command = _mapper.Map<UpdateMotocycleBikeCommand>(entity);            

            if (command != null && command.IsValid())
            {
                var toUpdate = command.UpdateMotocycleBike(entity, request.Plate);
                UpdateBusinessValidation(toUpdate);
                var result = await _repositoryMotocycleBike.Update(toUpdate);
                if (result == _notAffectedSet)
                {
                    throw new BusinessException(_invalid);
                }
                var response = _mapper.Map<ResponseMotocycleBike>(toUpdate);
                _redisCacheService.InvalidateCacheEntry("ApplicationServiceMotocycleBike.getAllmotos");
                _redisCacheService.InvalidateCacheEntry($"ApplicationServiceMotocycleBike.getById.{identifier}");
                _notify.NotifyMessage(response);
                return response;
            }
            var exceptionList = new StringBuilder();
            exceptionList.AppendLine(_invalid);
            if (command != null)
            {
                command.ValidationResult.Errors.ForEach(command => { exceptionList.AppendLine(command.ErrorMessage); });
            }
            throw new BusinessException(exceptionList.ToString());
        }

        public Task<List<ResponseMotocycleBike>> GetByPlate(string plate)
        {
            var inCacheStore = _redisCacheService.GetValue($"ApplicationServiceMotocycleBike.getByPlate.{plate}");
            if (string.IsNullOrEmpty(inCacheStore))
            {
                var entity = _repositoryMotocycleBike.GetAll().Where(p => p != null && p.Plate == plate).ToList();
                if (entity != null)
                {
                    var result = _mapper.Map<List<ResponseMotocycleBike>>(entity);
                    var serialized = JsonSerializer.Serialize(result);
                    _redisCacheService.SetValue($"ApplicationServiceMotocycleBike.getByPlate.{plate}", serialized);
                    return Task.FromResult(result);
                }
            }
            else
            {
                var result = JsonSerializer.Deserialize<List<ResponseMotocycleBike>>(inCacheStore);
#pragma warning disable CS8619
                return Task.FromResult(result);
#pragma warning restore CS8619
            }
#pragma warning disable CS8603
            return null;
#pragma warning restore CS8603 
        }        
    }
}
