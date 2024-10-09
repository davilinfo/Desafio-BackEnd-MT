using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interface
{
    public interface IApplicationServiceMotocycleBike : IApplicationService<ResponseMotocycleBike,RequestMotocycleAdd,RequestMotocycleUpdate>
    {
        public new Task<ResponseMotocycleBike> CreateAsync(RequestMotocycleAdd request);
        public new Task<ResponseMotocycleBike> UpdateAsync(string identifier, RequestMotocycleUpdate request);        
    }
}
