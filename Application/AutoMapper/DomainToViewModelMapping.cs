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
            CreateMap<Lease, LeaseViewModel>();            
            CreateMap<Lease, ResponseLease>();
        }
    }
}
