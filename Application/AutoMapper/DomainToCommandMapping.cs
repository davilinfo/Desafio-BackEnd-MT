using AutoMapper;
using Domain.Entities;
using Domain.MotocycleBike.Commands;

namespace Application.AutoMapper
{
    internal class DomainToCommandMapping : Profile
    {
        public DomainToCommandMapping() 
        { 
            CreateMap<MotocycleBike, UpdateMotocycleBikeCommand>();
        }
    }
}
