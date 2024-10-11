using Application.Models.Request;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    internal class RequestToDomainMapping : Profile
    {
        public RequestToDomainMapping() { 
            CreateMap<RequestMotocycleUpdate, MotocycleBike>();
        }
    }
}
