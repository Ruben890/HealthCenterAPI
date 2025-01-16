using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;

namespace HealthCenterAPI.Contracts.IServices
{
    public interface IFileServices
    {
        Task<BaseResponse> GetHealthCenter(GenericParameters parameters);
    }
}
