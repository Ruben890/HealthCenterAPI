using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.QueryParameters;

namespace HealthCenterAPI.Contracts.Iservices
{
    public interface IHealthCenterServices
    {
        Task<BaseResponse> GetHealthCenter(GenericParameters parameters);
    }
}
