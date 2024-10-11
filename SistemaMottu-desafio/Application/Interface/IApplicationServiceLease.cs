using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interface
{
    public interface IApplicationServiceLease : IApplicationService<ResponseLease, RequestLeaseAdd, RequestLeaseUpdate>
    {
        public new Task<ResponseLease> CreateAsync(RequestLeaseAdd requestLeaseAdd);
        public new Task<ResponseLease> UpdateAsync(string identifier, RequestLeaseUpdate requestLeaseUpdate);
    }
}
