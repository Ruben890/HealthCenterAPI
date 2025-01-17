using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;

namespace HealthCenterAPI.Domain.Contracts.IServices
{
    public interface IFileServices
    {
        Task<BaseResponse> GetHealthCenter(GenericParameters parameters);
    }
}
