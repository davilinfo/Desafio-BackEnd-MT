using Application.Models.Request;
using AutoMapper;
using Domain.Lease.Commands;
using Domain.MotocycleBike.Commands;

namespace Application.AutoMapper
{
    internal class RequestToCommandMapping : Profile
    {
        public RequestToCommandMapping() 
        {
            CreateMap<RequestMotocycleAdd, RegisterMotocycleBikeCommand>();
            CreateMap<RequestLeaseAdd, RegisterLeaseCommand>();
        }
    }
}
