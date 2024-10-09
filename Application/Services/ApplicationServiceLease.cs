using Application.Enum;
using Application.Exception;
using Application.Interface;
using Application.Models.Request;
using Application.Models.Response;
using AutoMapper;
using Domain.Contract;
using Domain.Entities;
using Domain.Lease.Commands;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Services
{
    public class ApplicationServiceLease : IApplicationServiceLease, IBusinessValidation<Lease>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryLease _repositoryLease;
        private readonly IRepositoryMotocycleBike _repositoryMotocycleBike;
        private readonly IRepositoryDeliver _repositoryDeliver;
        private readonly INotify<ResponseLease> _notify;
        private readonly ILogger<ApplicationServiceLease> _logger;        
        private readonly string _invalid = "Dados inválidos";
        private readonly string _notSaved = "Dados não gravados";        
        private readonly string _notFoundDeliver = "Entregador inexistente";
        private readonly string _notFoundMotocycleBike = "Moto inexistente";
        private readonly string _deliverLicenseInvalid = "Carteira de habilitação inválida";
        private readonly string _initialDateAboveEndDate = "Data início superior data termino";
        private readonly string _initialDateAbovePreviewEndDate = "Data início superior data termino";
        private readonly string _planSmallerThanPeriod = "Plano é menor do que o período entre data início e data termino";
        private readonly string _validLicenseTypeA = "A";
        private readonly string _validLicenseTypeAB = "A+B";
        private readonly int _notAffectedSet = 0;
        private readonly double _penaltyReturnAfterPreviewEndDay = 50;
        private readonly double _penaltyReturnBeforePreviewEndDay7Plan = 1.2;
        private readonly double _penaltyReturnBeforePreviewEndDay15Plan = 1.4;
        public ApplicationServiceLease(IMapper mapper, IRepositoryLease repositoryLease, IRepositoryDeliver repositoryDeliver, 
            IRepositoryMotocycleBike repositoryMotocycleBike,
            INotify<ResponseLease> notify, 
            ILogger<ApplicationServiceLease> logger) 
        { 
            _mapper = mapper;
            _repositoryLease = repositoryLease;
            _repositoryDeliver = repositoryDeliver;
            _repositoryMotocycleBike = repositoryMotocycleBike;
            _notify = notify;
            _logger = logger;
        }
        public async Task<ResponseLease> CreateAsync(RequestLeaseAdd requestLeaseAdd)
        {
            var command = _mapper.Map<RegisterLeaseCommand>(requestLeaseAdd);
            if (command.IsValid())
            {
                var entity = command.CreateLease();
                RegisterBusinessValidation(entity);
                SetPlanValue(ref entity);

                var result = await _repositoryLease.Add(entity);
                var response = _mapper.Map<ResponseLease>(entity);

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

        public Task DeleteAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseLease>> GetAllAsync()
        {
            var result = from item in _repositoryLease.GetAll()
                         select new ResponseLease
                         {
                             Identifier = item.Identifier,
                             DeliverId= item.DeliverId,
                             MotocycleBikeId= item.MotocycleBikeId,
                             InitialDate = item.InitialDate,
                             EndDate = item.EndDate,
                             DevolutionDate= item.DevolutionDate,
                             PreviewEndDate= item.PreviewEndDate,
                             Value = item.Value
                         };

            return Task.FromResult(result.ToList());
        }

        public async Task<ResponseLease> GetById(string identifier)
        {
            var entity = await _repositoryLease.GetById(identifier);
            if (entity != null)
            {
                var result = _mapper.Map<ResponseLease>(entity);
                return result;
            }
#pragma warning disable CS8603 
            return null;
#pragma warning restore CS8603
        }

        public async Task<ResponseLease> UpdateAsync(string identifier, RequestLeaseUpdate requestLeaseUpdate)
        {
            var entity = await _repositoryLease.GetById(identifier);
            var command = _mapper.Map<UpdateLeaseCommand>(entity);
            var modelEntity = _mapper.Map<Lease>(requestLeaseUpdate);

            if (command != null && command.IsValid())
            {
                var toUpdate = command.UpdateLease(modelEntity);
                UpdateBusinessValidation(toUpdate);
                SetValueDevolution(ref toUpdate);
                var result = await _repositoryLease.Update(toUpdate);
                if (result == _notAffectedSet)
                {
                    throw new BusinessException(_invalid);
                }
                var response = _mapper.Map<ResponseLease>(toUpdate);
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

        public void RegisterBusinessValidation(Lease entity)
        {
            var deliver = _repositoryDeliver.GetAll().FirstOrDefault(p=> p.Identifier == entity.DeliverId);
            if (deliver == null)
            {
                throw new BusinessException(_notFoundDeliver);
            }
            if (!(deliver.DriverLicenseType.Equals(_validLicenseTypeA) || deliver.DriverLicenseType.Equals(_validLicenseTypeAB)))
            {
                throw new BusinessException(_deliverLicenseInvalid);
            }
            var moto = _repositoryMotocycleBike.GetAll().FirstOrDefault(p => p.Identifier == entity.MotocycleBikeId);
            if (moto == null)
            {
                throw new BusinessException (_notFoundMotocycleBike);
            }
            if (entity.InitialDate.Date > entity.EndDate.Date)
            {
                throw new BusinessException(_initialDateAboveEndDate);
            }
            if (entity.InitialDate.Date > entity.PreviewEndDate.Date)
            {
                throw new BusinessException(_initialDateAbovePreviewEndDate);
            }
            if (entity.Plan < (entity.EndDate.Date.Subtract(entity.InitialDate.Date).Days))
            {
                throw new BusinessException(_planSmallerThanPeriod);
            }
        }        

        public void UpdateBusinessValidation(Lease entity)
        {
            var lease = _repositoryLease.GetAll().FirstOrDefault(p=> p.Identifier == entity.Identifier);
            if (lease == null)
            {
                throw new BusinessException(_notFoundDeliver);
            }
        }

        private void SetPlanValue(ref Lease entity)
        {
            var planType = (PlanType)entity.Plan;
            switch (planType)
            {
                case PlanType.SevenDays:
                    entity.Value = (double)PlanValueType.SevenDays;
                    break;
                case PlanType.FifteenDays:
                    entity.Value = (double)PlanValueType.FifteenDays;
                    break;
                case PlanType.ThirtyDays:
                    entity.Value = (double)PlanValueType.ThirtyDays;
                    break;
                case PlanType.FortyFiveDays:
                    entity.Value = (double)PlanValueType.FortyFiveDays;
                    break;
                case PlanType.FiftyDays:
                    entity.Value = (double)PlanValueType.FiftyDays;
                    break;
                default:
                    entity.Value = (double)PlanValueType.FiftyDays;
                    break;
            }
        }

        private void SetValueDevolution(ref Lease entity)
        {
            var identifer = entity.Identifier;
            var lease = _repositoryLease.GetAll().FirstOrDefault(p => p.Identifier == identifer);
#pragma warning disable CS8629
            var devolutionDate = entity.DevolutionDate.Value.Date;
            var previewEndDate = entity.PreviewEndDate.Date;
#pragma warning restore CS8629
            if (devolutionDate > previewEndDate)
            {
                var days = devolutionDate.Subtract(previewEndDate).Days;
                var calcPenalty = days * _penaltyReturnAfterPreviewEndDay;
                var calcNormalDays = entity.Value * entity.Plan;
                var total = (calcPenalty + calcNormalDays) / entity.Plan;
                entity.Value = total;
            }
            if (devolutionDate < previewEndDate && entity.Plan == (int)PlanType.SevenDays)
            {
                var days = previewEndDate.Subtract(devolutionDate).Days;
                var calcPenalty = days * _penaltyReturnBeforePreviewEndDay7Plan;
                var calcNormalDays = (entity.Plan - days) * (double)PlanValueType.SevenDays;
                var total = (calcPenalty + calcNormalDays) / entity.Plan;
                entity.Value = total;
            }
            if (devolutionDate < previewEndDate && entity.Plan == (int)PlanType.FifteenDays)
            {
                var days = previewEndDate.Subtract(devolutionDate).Days;                
                var calcPenalty = days * _penaltyReturnBeforePreviewEndDay15Plan;
                var calcNormalDays = (entity.Plan - days) * (double)PlanValueType.FifteenDays;
                var total = (calcPenalty + calcNormalDays) / entity.Plan;
                entity.Value = total;
            }
        }
    }
}
