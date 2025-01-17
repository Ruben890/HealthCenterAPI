using HealthCenterAPI.Domain.Contracts.IServices;
using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace HealthCenterAPI.Presentation.Controllers
{
    [Route("api/HealthCenter")]
    [ApiController]
    public class HealthCenterController : ControllerBase
    {
        private readonly IFileServices _fileService;
        private readonly IHealthCenterServices _healthCenterServices;
        private readonly IConfiguration _configuration;
        public HealthCenterController
            (
            IFileServices services,
            IConfiguration configuration,
            IHealthCenterServices healthCenterServices
            )
        {
            _fileService = services;
            _configuration = configuration;
            _healthCenterServices = healthCenterServices;
        }


        [HttpGet("GetAllHealthCenter")]
        public async Task<IActionResult> GetAllHealthCenter([FromQuery] GenericParameters parameters)
        {
            var response = new BaseResponse();
            try
            {
                if (_configuration.GetValue("DataSourceType:Database", false) && parameters.SourceType == DataSourceType.Database)
                {
                    return Ok(await _healthCenterServices.GetHealthCenter(parameters));
                }
                else
                {
                    return Ok(await _fileService.GetHealthCenter(parameters));
                }

            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
