using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.FinPrograms;
using RSCS.FinancingStatements.Core.Interfaces.Service;
using RSCS.FinancingStatements.Core.Models.RequestParameters;
using RSCS.FinancingStatements.Shared.ResponseHandler;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace RSCS.FinancingStatements.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramMaintenanceController : ControllerBase
    {
        private readonly ILogger<ProgramMaintenanceController> _logger;
        private readonly IFinProgramsService _finProgramsService;

        public ProgramMaintenanceController(ILogger<ProgramMaintenanceController> logger, IFinProgramsService finProgramsService)
        {
            _logger = logger;
            _finProgramsService = finProgramsService;
        }
        [HttpGet]
        [Route("getFinPrograms")]
        public APIResponse getFinPrograms()
        {
            try
            {
                _logger.LogInformation($"getFinPrograms is fetched");
                IEnumerable<FinPrograms> finPrograms = _finProgramsService.FetchFinprograms().Result;
                return new APIResponse((int)HttpStatusCode.OK, "FinProgram list", finPrograms);
            }
            catch (Exception ex)
            {
                ApiError apiError = new ApiError(ex.Message);
                _logger.LogError($"getFinPrograms has error : " + apiError.ExceptionMessage);
                return new APIResponse((int)HttpStatusCode.ExpectationFailed, "FinProgram List", null, apiError);
            }
        }

        [HttpPut]
        [Route("UpdateFinPrograms")]
        public APIResponse UpdateFinPrograms(FinProgramRequestParameter finPrograms)
        {
            try
            {
                _logger.LogInformation($"UpdateFinPrograms is fetched");
                int UpdatedFinProgram = _finProgramsService.UpdateFinProgram(finPrograms).Result;
                return new APIResponse((int)HttpStatusCode.OK, "Updated Successfully", UpdatedFinProgram);
            }
            catch (Exception ex)
            {
                ApiError apiError = new ApiError(ex.Message);
                _logger.LogError($"UpdateFinPrograms has error : " + apiError.ExceptionMessage);
                return new APIResponse((int)HttpStatusCode.ExpectationFailed, "Error in updating", null, apiError);
            }
        }

    }
}
