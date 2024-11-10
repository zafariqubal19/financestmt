using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using RSCS.FinancingStatements.Web.Facade;
using RSCS.FinancingStatements.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using SmartBreadcrumbs.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RSCS.FinancingStatements.Web.Controllers
{
    [Breadcrumb]
    public class ProgramMaintenanceController : Controller
    {
        private readonly IApplicationFacade _applicationFacade;
        private readonly ILogger<ProgramMaintenanceController> _logger;
        private readonly IMemoryCache _memoryCache;

        public ProgramMaintenanceController(ILogger<ProgramMaintenanceController> logger, IApplicationFacade applicationFacade, IMemoryCache memoryCache)
        {
            _logger = logger;
            _applicationFacade = applicationFacade;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return RedirectToAction("GetProgramMaintenace");
        }


        public IActionResult GetProgramMaintenance()
        {
            _logger.LogInformation($"GetFinProgramsFranchisee initiated by user - " + User.Identity?.Name);
            return View();
        }


        public ActionResult FetchFinprograms([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IEnumerable<FinPrograms> finPrograms = _applicationFacade.FetchProgramMaintenanceFinPrograms().Result;
                var dsRequest = finPrograms.ToDataSourceResult(request);
                return Json(dsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Finprograms.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching Finprograms. Please try again later.");
            }
        }

        [HttpPost]
        public ActionResult UpdateFinprograms(FinProgramRequestParameter finProgram)
        {
            try
            {
                FinProgramRequestParameter finProgramRequestParameter = new FinProgramRequestParameter();
                if (finProgram.PaymentTermsCode == null)
                {
                    finProgram.PaymentTermsCode = "";
                }
                if (_applicationFacade.UpdateFinPrograms(finProgram).Result > 0)
                {
                    TempData["updated"] = "Updated";

                }
                _logger.LogInformation($"UpdateFinprograms initiated by user - " + User.Identity?.Name);
                return RedirectToAction("GetProgramMaintenance");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching UpdateFinprograms.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching UpdateFinprograms. Please try again later.");
            }

        }
        public IEnumerable<FinPrograms> FetchAllFinPrograms()
        {
            IEnumerable<FinPrograms> finPrograms = _applicationFacade.FetchProgramMaintenanceFinPrograms().Result;
            return finPrograms;
        }

    }
}
