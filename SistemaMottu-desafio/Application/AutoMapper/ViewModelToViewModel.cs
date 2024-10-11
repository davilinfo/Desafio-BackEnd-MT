using Application.Models.Response;
using Application.Models.ViewModel;
using AutoMapper;

namespace Application.AutoMapper
{
    internal class ViewModelToViewModel : Profile
    {
        public ViewModelToViewModel() 
        { 
            CreateMap<ResponseMotocycleBike, MessageMoto>();         
            CreateMap<ResponseDeliver, MessageDeliver>();
            CreateMap<ResponseLease, MessageLease>();
        }
    }
}
