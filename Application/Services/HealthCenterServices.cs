using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared;
using HealthCenterAPI.Domain.Contracts.IRepository;
using HealthCenterAPI.Domain.Contracts.IServices;

namespace HealthCenterAPI.Application.Services
{
    public class HealthCenterServices : IHealthCenterServices
    {
        private readonly IHealthCenterRepository _healthCenterRepository;

        public HealthCenterServices(IHealthCenterRepository healthCenterRepository)
        {
            _healthCenterRepository = healthCenterRepository;
        }


        public async Task<BaseResponse> GetHealthCenter(GenericParameters parameters)
        {
            var response = new BaseResponse();
            try
            {
                var healthCenter = await _healthCenterRepository.GetAllHealthCenter(parameters);
                response.SetPagination(healthCenter.Pagination);
                response.Details = healthCenter;
            }
            catch (Exception)
            {

                throw;
            }

            return response;
        }
    }
}
