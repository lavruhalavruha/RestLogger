using System;
using System.Threading.Tasks;
using System.Web.Http;

using RestLogger.Infrastructure.Service;
using RestLogger.Infrastructure.Service.Model.ApplicationDtos;
using RestLogger.Infrastructure.Service.Model.LogDtos;
using RestLogger.WebApi.Filters;
using RestLogger.WebApi.Helpers.Mappers.ApplicationMapper;
using RestLogger.WebApi.Helpers.Mappers.LogMapper;
using RestLogger.WebApi.Models.ApplicationModels;
using RestLogger.WebApi.Models.LogModels;

namespace RestLogger.WebApi.Controllers
{
    [RequireHttps]
    public class LoggerController : ApiController
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogService _logService;

        public LoggerController(IApplicationService applicationService,
            ILogService logService)
        {
            _applicationService = applicationService;
            _logService = logService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterAsync(ApplicationCreateModel applicationCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationCreateDto applicationCreateDto = applicationCreateModel.ToApplicationCreateDto();

                int applicationId = await _applicationService.AddApplicationAsync(applicationCreateDto);

                return Ok(applicationId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("log")]
        public async Task<IHttpActionResult> LogAsync(LogCreateModel logCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LogCreateDto logCreateDto = logCreateModel.ToLogCreateDto();

            try
            {
                int id = await _logService.AddLogAsync(logCreateDto, User.Identity.Name);

                return Ok(new LogCreationResultModel()
                {
                    success = id > 0
                });
            }
            catch (Exception)
            {
                return Ok(new LogCreationResultModel()
                {
                    success = false
                });
            }
        }
    }
}
