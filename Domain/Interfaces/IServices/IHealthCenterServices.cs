using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.QueryParameters;

namespace HealthCenterAPI.Domain.Contracts.IServices
{
    public interface IHealthCenterServices
    {
        Task<BaseResponse> GetHealthCenter(GenericParameters parameters);
    }
}
