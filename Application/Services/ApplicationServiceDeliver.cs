using Application.Exception;
using Application.Interface;
using Application.Models.Request;
using Application.Models.Response;
using AutoMapper;
using Domain.Contract;
using Domain.Deliver.Commands;
using Domain.Entities;
using Ipfs.Engine;
using System.Text;

namespace Application.Services
{
    public class ApplicationServiceDeliver : IApplicationServiceDeliver, IBusinessValidation<Deliver>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryDeliver _repositoryDeliver;        
        private readonly INotify<Deliver> _notify;
        private readonly string _notSaved = "Dados não gravados";
        private readonly string _invalid = "Dados inválidos";
        private readonly string _foundDriverLicenseNumber = "Já existe cnh cadastrada";
        private readonly string _foundDeliverUniqueIdentifier = "Já existe cnpj cadastrado";
        private readonly int _notAffectedSet = 0;
        public ApplicationServiceDeliver(IMapper mapper, IRepositoryDeliver repositoryDeliver, INotify<Deliver> notify) 
        { 
            _mapper = mapper;
            _repositoryDeliver = repositoryDeliver;
            _notify = notify;
        }
        public async Task<ResponseDeliver> CreateAsync(RequestDeliverAdd requestDeliverAdd)
        {
            var command = _mapper.Map<RegisterDeliverCommand>(requestDeliverAdd);
            if (command.IsValid())
            {                
                var entity = command.CreateDeliver();
                entity.DriverLicenseImageS3 = await AddToIPFS(entity.DriverLicenseImage);
                RegisterBusinessValidation(entity);
                var result = await _repositoryDeliver.Add(entity);
                var response = _mapper.Map<ResponseDeliver>(entity);
                if (!string.IsNullOrEmpty(entity.Identifier))
                {
                    _notify.NotifyMessage(entity);
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

        public Task<List<ResponseDeliver>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDeliver> GetById(string identifier)
        {
            throw new NotImplementedException();
        }

        public void RegisterBusinessValidation(Deliver entity)
        {
            var anyDriverLicenseNumber = _repositoryDeliver.GetAll().Any(p => p.DriverLicenseNumber == entity.DriverLicenseNumber);
            if (anyDriverLicenseNumber)
            {
                throw new BusinessException(_foundDriverLicenseNumber);
            }
            var anyDeliverUniqueIdentifier = _repositoryDeliver.GetAll().Any(p => p.UniqueIdentifier == entity.UniqueIdentifier);
            if (anyDeliverUniqueIdentifier)
            {
                throw new BusinessException(_foundDeliverUniqueIdentifier);
            }
        }

        public async Task<ResponseDeliver> UpdateAsync(string identifier, RequestDeliverUpdate requestDeliverUpdate)
        {
            var entity = await _repositoryDeliver.GetById(identifier);
            var command = _mapper.Map<UpdateDeliverCommand>(entity);
            var modelEntity = _mapper.Map<Deliver>(requestDeliverUpdate);

            if (command != null && command.IsValid())
            {
                modelEntity.DriverLicenseImageS3 = await AddToIPFS(modelEntity.DriverLicenseImage);
                var toUpdate = command.UpdateDeliver(modelEntity);                
                UpdateBusinessValidation(toUpdate);
                
                var result = await _repositoryDeliver.Update(toUpdate);
                if (result == _notAffectedSet)
                {
                    throw new BusinessException(_invalid);
                }
                var response = _mapper.Map<ResponseDeliver>(toUpdate);
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

        public void UpdateBusinessValidation(Deliver entity)
        {            
        }

        private async Task<string> AddToIPFS(string base64string)
        {
            var ipfs = new IpfsEngine();
            
            byte[] data = Convert.FromBase64String(base64string);

            using (var fs = new MemoryStream(data))
            {                
                var node = await ipfs.FileSystem.AddAsync(fs);
                fs.Close();
                return node.Id;                
            }                            
        }
    }
}
