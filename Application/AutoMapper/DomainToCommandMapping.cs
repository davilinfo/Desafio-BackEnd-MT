using AutoMapper;
using Domain.Deliver.Commands;
using Domain.Entities;
using Domain.Lease.Commands;
using Domain.MotocycleBike.Commands;

namespace Application.AutoMapper
{
    internal class DomainToCommandMapping : Profile
    {
        public DomainToCommandMapping() 
        { 
            CreateMap<MotocycleBike, UpdateMotocycleBikeCommand>();
            CreateMap<Deliver, UpdateDeliverCommand>();
            CreateMap<Lease, UpdateLeaseCommand>();
        }
    }
}
