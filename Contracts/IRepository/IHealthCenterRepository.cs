using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;

namespace HealthCenterAPI.Contracts.IRepository
{
    public interface IHealthCenterRepository
    {
        Task<PagedList<HealthCenterDto>> GetAllHealthCenter(GenericParameters parameters);
    }
}
