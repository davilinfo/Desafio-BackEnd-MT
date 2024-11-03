using Application.Exception;
using Application.Interface;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Contract;
using Domain.Deliver.Commands;
using Domain.Entities;
using Ipfs.Engine;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Services
{
    public class ApplicationServiceDeliver : IApplicationServiceDeliver, IBusinessValidation<Deliver>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryDeliver _repositoryDeliver;        
        private readonly INotify<MessageDeliver> _notify;
        private readonly ILogger<ApplicationServiceDeliver> _logger;
        private readonly char[] _passphrase = "$2b@D9f!kL7mP#qR8sT1vX4z".ToCharArray();
        private readonly string _notSaved = "Dados não gravados";
        private readonly string _invalid = "Dados inválidos";
        private readonly string _foundDriverLicenseNumber = "Já existe cnh cadastrada";
        private readonly string _foundDeliverUniqueIdentifier = "Já existe cnpj cadastrado";
        private readonly string _foundDeliverIdentifier = "Já existe identificador cadastrado";
        private readonly string _retrievedBase64String = "Retrieved Base64 string:";
        private readonly string _addedToIPFS = "Added Base64 string to IPFS:";
        private readonly string _addAction = "add";
        private readonly string _updateAction = "update";
        private readonly string _removeAction = "remove";
        private readonly int _notAffectedSet = 0;
        public ApplicationServiceDeliver(IMapper mapper, IRepositoryDeliver repositoryDeliver, 
            INotify<MessageDeliver> notify, ILogger<ApplicationServiceDeliver> logger) 
        { 
            _mapper = mapper;
            _repositoryDeliver = repositoryDeliver;
            _notify = notify;
            _logger = logger;
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
                    var message = _mapper.Map<MessageDeliver>(entity);
                    message.Action = _addAction;
                    _notify.NotifyMessage(message);
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
            var anyDeliverIdentifier = _repositoryDeliver.GetAll().Any(p => p.Identifier == entity.Identifier);
            if (anyDeliverIdentifier)
            {
                throw new BusinessException(_foundDeliverIdentifier);
            }
        }

        public async Task<ResponseDeliver> UpdateAsync(string identifier, RequestDeliverUpdate requestDeliverUpdate)
        {
            var entity = await _repositoryDeliver.GetById(identifier);
            var command = _mapper.Map<UpdateDeliverCommand>(entity);            

            if (command != null && command.IsValid())
            {
                var newImageRef = await AddToIPFS(requestDeliverUpdate.DriverLicenseImage);
                var toUpdate = command.UpdateDeliver(entity, requestDeliverUpdate.DriverLicenseImage, newImageRef);                
                UpdateBusinessValidation(toUpdate);
                
                var result = await _repositoryDeliver.Update(toUpdate);
                if (result == _notAffectedSet)
                {
                    throw new BusinessException(_invalid);
                }
                var response = _mapper.Map<ResponseDeliver>(toUpdate);
                var message = _mapper.Map<MessageDeliver>(toUpdate);
                message.Action = _updateAction;
                _notify.NotifyMessage(message);
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
            var ipfs = new IpfsEngine(_passphrase);
            
            byte[] data = Convert.FromBase64String(base64string);

            using (var fs = new MemoryStream(data))
            {                
                var node = await ipfs.FileSystem.AddAsync(fs);
                _logger.LogInformation($"{_addedToIPFS} {base64string} {node.Id}");
                
                fs.Close();
                return node.Id;                
            }                            
        }

        private async Task<string> GetFromIPFS(string cid)
        {
            var ipfs = new IpfsEngine();
            var retrievedData = await ipfs.FileSystem.ReadFileAsync(cid);
            var data = new byte[retrievedData.Length];
            retrievedData.Write(data, 0, data.Length);
            string retrievedBase64String = Convert.ToBase64String(data);
            _logger.LogInformation($"{_retrievedBase64String} {retrievedBase64String}");
            return retrievedBase64String;
        }
    }
}
