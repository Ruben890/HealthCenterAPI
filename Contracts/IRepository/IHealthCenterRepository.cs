using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;

namespace HealthCenterAPI.Contracts.IRepository
{
    public interface IHealthCenterRepository
    {
        Task<PagedList<HealthCenterDto>> GetAllHealthCenter(GenericParameters parameters);
        Task<HealthCenter> GetbyNameHealthCenter(string name);
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;
        void InsertRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class;
    }
}
