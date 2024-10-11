using Application.Models.Response;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    internal class DomainToViewModelMapping : Profile
    {
        public DomainToViewModelMapping() 
        { 
            CreateMap<MotocycleBike, MotocycleBikeViewModel>();
            CreateMap<MotocycleBike, ResponseMotocycleBike>();
            CreateMap<MotocycleBike, MessageMoto>();
            CreateMap<Lease, LeaseViewModel>();            
            CreateMap<Lease, ResponseLease>();
            CreateMap<Lease, MessageLease>();
            CreateMap<Deliver, DeliverViewModel>();
            CreateMap<Deliver, ResponseDeliver>();
            CreateMap<Deliver, MessageDeliver>();    
        }
    }
}
