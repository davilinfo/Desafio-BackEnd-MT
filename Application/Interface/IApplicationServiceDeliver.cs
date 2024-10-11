using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interface
{
    public interface IApplicationServiceDeliver : IApplicationService<ResponseDeliver, RequestDeliverAdd, RequestDeliverUpdate>
    {
        public new Task<ResponseDeliver> CreateAsync(RequestDeliverAdd requestDeliverAdd);
        public new Task<ResponseDeliver> UpdateAsync(string identifier, RequestDeliverUpdate requestDeliverUpdate);
    }
}
