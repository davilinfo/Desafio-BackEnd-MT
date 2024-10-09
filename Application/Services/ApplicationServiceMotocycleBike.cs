using Application.Exception;
using Application.Interface;
using Application.Models.Request;
using Application.Models.Response;
using AutoMapper;
using Domain.Contract;
using Domain.Entities;
using Domain.MotocycleBike.Commands;
using System.Text;

namespace Application.Services
{
    public class ApplicationServiceMotocycleBike : IApplicationServiceMotocycleBike, IBusinessValidation<MotocycleBike>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryMotocycleBike _repositoryMotocycleBike;
        private readonly IRepositoryLease _repositoryLease;
        private readonly INotify<ResponseMotocycleBike> _notify;
        private readonly string _plateFound = "Placa existente";
        private readonly string _invalid = "Dados inválidos";
        private readonly string _notSaved = "Dados não gravados";
        private readonly string _existMotoLease = "Existe registro de locação de moto";
        private readonly int _notAffectedSet = 0;
        public ApplicationServiceMotocycleBike(IMapper mapper, 
            IRepositoryMotocycleBike repositoryMotocycleBike, 
            IRepositoryLease repositoryLease,
            INotify<ResponseMotocycleBike> notify) 
        { 
            _mapper = mapper;
            _repositoryMotocycleBike = repositoryMotocycleBike;
            _repositoryLease = repositoryLease;
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
            var anyMoto = _repositoryLease.GetAll().Any(p=>p.MotocycleBikeId == identifier);
            if (anyMoto)
            {
                throw new BusinessException(_existMotoLease);
            }
            var result = await _repositoryMotocycleBike.Delete(identifier);
            
            if (result == _notAffectedSet)
            {
                throw new BusinessException(_invalid);
            }
        }

        public Task<List<ResponseMotocycleBike>> GetAllAsync()
        {
            
            var result = from item in _repositoryMotocycleBike.GetAll()
                         select new ResponseMotocycleBike 
                         { 
                             Identifier = item.Identifier, 
                             Model = item.Model, 
                             Plate = item.Plate, 
                             Year = item.Year 
                         };
            
            return Task.FromResult(result.ToList());
        }

        public async Task<ResponseMotocycleBike> GetById(string identifier)
        {
            var entity = await _repositoryMotocycleBike.GetById(identifier);
            if (entity != null)
            {
                var result = _mapper.Map<ResponseMotocycleBike>(entity);
                return result;
            }
#pragma warning disable CS8603 
            return null;
#pragma warning restore CS8603 
        }                

        public void RegisterBusinessValidation(MotocycleBike entity)
        {
            var result = _repositoryMotocycleBike.GetAll().Any(p => p.Plate.Equals(entity.Plate, StringComparison.CurrentCultureIgnoreCase));
            if (result)
            {
                throw new BusinessException(_plateFound);
            }
        }

        public void UpdateBusinessValidation(MotocycleBike entity)
        {
            var result = _repositoryMotocycleBike.GetAll().Any(p => p.Plate.Equals(entity.Plate, StringComparison.CurrentCultureIgnoreCase));
            if (result)
            {
                throw new BusinessException(_plateFound);
            }
        }

        public async Task<ResponseMotocycleBike> UpdateAsync(string identifier, RequestMotocycleUpdate request)
        {
            var entity = await _repositoryMotocycleBike.GetById(identifier);
            var command = _mapper.Map<UpdateMotocycleBikeCommand>(entity);
            var modelEntity = _mapper.Map<MotocycleBike>(request);

            if (command != null && command.IsValid())
            {
                var toUpdate = command.UpdateMotocycleBike(modelEntity);
                UpdateBusinessValidation(toUpdate);
                var result = await _repositoryMotocycleBike.Update(toUpdate);
                if (result == _notAffectedSet)
                {
                    throw new BusinessException(_invalid);
                }
                var response = _mapper.Map<ResponseMotocycleBike>(toUpdate);
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

        public Task<ResponseMotocycleBike> GetByPlate(string plate)
        {
            var entity = _repositoryMotocycleBike.GetAll().FirstOrDefault(p=> p.Plate == plate);
            if (entity != null)
            {
                var result = _mapper.Map<ResponseMotocycleBike>(entity);
                return Task.FromResult(result);
            }
#pragma warning disable CS8603 
            return null;
#pragma warning restore CS8603 
        }        
    }
}
