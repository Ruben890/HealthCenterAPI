using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;

namespace HealthCenterAPI.Domain.Contracts.IRepository
{
    public interface IHealthCenterRepository
    {
        Task<PagedList<HealthCenterDto>> GetAllHealthCenter(GenericParameters parameters);
        Task<List<string>> GetExistingNamesAsync(List<string> names);
        Task InserRangeAsync<T>(IEnumerable<T> entities) where T : class;
    }
}
