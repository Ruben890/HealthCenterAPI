using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;

namespace HealthCenterAPI.Domain.Contracts.IRepository
{
    public interface IFileRepository
    {
        List<HealthCenterDto> FilterHealthCenters(List<HealthCenterDto> healthCenters, GenericParameters parameters);
        Task<List<HealthCenterDto>> MapExcelToPagedDto(GenericParameters? parameters = null);
    }
}
