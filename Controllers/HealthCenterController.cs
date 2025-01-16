using HealthCenterAPI.Contracts.IServices;
using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace HealthCenterAPI.Controllers
{
    [Route("api/HealthCenter")]
    [ApiController]
    public class HealthCenterController : ControllerBase
    {
        private readonly IFileServices _fileService;
        private readonly IConfiguration _configuration;
        public HealthCenterController(IFileServices services, IConfiguration configuration)
        {
            _fileService = services;
            _configuration = configuration;
        }


        [HttpGet("GetAllHealthCenter")]
        public async Task<IActionResult> GetAllHealthCenter([FromQuery] GenericParameters parameters)
        {
            var response = new BaseResponse();
            try
            {
                return Ok(await _fileService.GetHealthCenter(parameters));
            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
