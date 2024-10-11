using Application.Models.ViewModel;
using AutoMapper;
using Domain.Deliver.Commands;
using Domain.Lease.Commands;
using Domain.MotocycleBike.Commands;

namespace Application.AutoMapper
{
    internal class ViewModelToCommandMapping : Profile
    {
        public ViewModelToCommandMapping() 
        { 
            CreateMap<MotocycleBikeViewModel, RegisterMotocycleBikeCommand>();            
            CreateMap<DeliverViewModel, RegisterDeliverCommand>();
            CreateMap<LeaseViewModel, RegisterLeaseCommand>();            
        }
    }
}
