using Application.Models.Request;
using Application.Models.ViewModel;
using AutoMapper;

namespace Application.AutoMapper
{
    internal class RequestToViewModelMapping : Profile
    {
        public RequestToViewModelMapping() 
        {
            CreateMap<RequestMotocycleAdd, MotocycleBikeViewModel>();
            CreateMap<RequestDeliverAdd, DeliverViewModel>();
            CreateMap<RequestLeaseAdd, LeaseViewModel>();
        }
    }
}
