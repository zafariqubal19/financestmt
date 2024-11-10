using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.FinPrograms;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.FinProgramsFranchisee;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.InvoiceDetails;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.StatementDetails;
using RSCS.FinancingStatements.Core.Services;
using RSCS.FinancingStatements.Core.Interfaces.Service;
using RSCS.FinancingStatements.Shared.ResponseHandler;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace RSCS.FinancingStatements.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IFinProgramsService _finProgramsService;
        private readonly IFinProgramsFranchiseeService _finProgramsFranchiseeService;
        private readonly IInvoiceDetailsService _invoiceDetailsService;
        private readonly IStatementDetailsService _StatementDetailsService;

        public InvoiceController(ILogger<InvoiceController> logger, IFinProgramsService finProgramsService,
            IFinProgramsFranchiseeService finProgramsFranchiseeService, IInvoiceDetailsService invoiceDetailsService,
            IStatementDetailsService statementDetailsService)
        {
            _logger = logger;
            _finProgramsService = finProgramsService;
            _finProgramsFranchiseeService = finProgramsFranchiseeService;
            _invoiceDetailsService = invoiceDetailsService;
            _StatementDetailsService = statementDetailsService;

        }

        [HttpGet]
        [Route("getFinPrograms")]
        public APIResponse getFinPrograms()
        {

            try
            {
                _logger.LogInformation($"getFinPrograms is fetched");
                IEnumerable<FinPrograms> finPrograms = _finProgramsService.FetchFinprograms().Result;
                return new APIResponse((int)HttpStatusCode.OK, "FinProgramList", finPrograms);

            }
            catch (Exception ex)
            {
                ApiError apiError = new ApiError(ex.Message);
                _logger.LogError($"getFinPrograms has error : " + apiError.ExceptionMessage);
                return new APIResponse((int)HttpStatusCode.ExpectationFailed, "ProgramList", null, apiError);

            }


        }
        [HttpGet]
        [Route("getFinProgramsFranchisee")]
        public APIResponse GetFinProgramsFranchisees(int ProgramId)
        {
            try
            {
                _logger.LogInformation($"GetFinProgramsFranchisees is fetched");
                IEnumerable<FinProgramsFranchisee> finProgramsFranchisees = _finProgramsFranchiseeService.FetchFinProgramFranchisee(ProgramId).Result;
                return new APIResponse((int)HttpStatusCode.OK, "Franchisee list", finProgramsFranchisees);
            }
            catch (Exception ex)
            {
                ApiError apiError = new ApiError(ex.Message);
                _logger.LogError($"GetFinProgramsFranchisees has error : " + apiError.ExceptionMessage);
                return new APIResponse((int)HttpStatusCode.ExpectationFailed, "Franchisee list", null, apiError);

            }

        }
        [HttpGet]
        [Route("getInvoiceDetails")]
        public APIResponse GetInvoiceDetails(int programId, string masterId)
        {
            try
            {
                _logger.LogInformation($"GetInvoiceDetails is fetched");
                IEnumerable<InvoiceDetails> invoiceDetails = _invoiceDetailsService.FetchInvoiceDetails(programId, masterId).Result;
                return new APIResponse((int)HttpStatusCode.OK, "Invoice List", invoiceDetails);
            }
            catch (Exception ex)
            {
                ApiError apiError = new ApiError(ex.Message);
                _logger.LogError($"GetInvoiceDetails has error : " + apiError.ExceptionMessage);
                return new APIResponse((int)HttpStatusCode.ExpectationFailed, "Invoice list", null, apiError);
            }
        }
        [HttpGet]
        [Route("getStatementDetails")]
        public APIResponse GetStatementDetails(int programId, string masterId)
        {
            try
            {
                _logger.LogInformation($"GetStatementDetails is fetched");
                IEnumerable<StatementDetails> statementDetails = _StatementDetailsService.FetchStatementDetails(programId, masterId).Result;
                return new APIResponse((int)HttpStatusCode.OK, "Statement List", statementDetails);
            }
            catch (Exception ex)
            {
                ApiError apiError = new ApiError(ex.Message);
                _logger.LogError($"GetStatementDetails has error : " + apiError.ExceptionMessage);
                return new APIResponse((int)HttpStatusCode.ExpectationFailed, "Statement List", null, apiError);
            }
        }

    }
}
