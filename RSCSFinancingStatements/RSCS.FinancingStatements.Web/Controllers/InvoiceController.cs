using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using RSCS.FinancingStatements.Web.Facade;
using RSCS.FinancingStatements.Web.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Mime;
using Telerik.SvgIcons;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Printing;
using MessagePack;
using SmartBreadcrumbs.Attributes;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;

namespace RSCS.FinancingStatements.Web.Controllers
{
    [DefaultBreadcrumb("Home")]
    public class InvoiceController : Controller
    {
        private readonly IApplicationFacade _applicationFacade;
        private readonly ILogger<InvoiceController> _logger;
        private readonly IMemoryCache _memoryCache;

        public InvoiceController(ILogger<InvoiceController> logger, IApplicationFacade applicationFacade, IMemoryCache memoryCache)
        {
            _logger = logger;
            _applicationFacade = applicationFacade;
            _memoryCache = memoryCache;
        }
        public ActionResult Index() 
        {
            return RedirectToAction("GetFinPrograms"); 
        }
        [Breadcrumb(Title = "FinPrograms")]
        [HttpGet]
        public IActionResult GetFinPrograms()
        {
            _logger.LogInformation($"GetFinPrograms initiated by user - " + User.Identity?.Name);
            return View();
        }

        public ActionResult FetchFinprograms([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                if (_memoryCache.TryGetValue("FinProgramsCache", out IEnumerable<FinPrograms> cachedData))
                {
                    var dsRequestCatche = cachedData.ToDataSourceResult(request);
                    return Json(dsRequestCatche);
                }

                IEnumerable<FinPrograms> finPrograms = _applicationFacade.FetchFinPrograms().Result;
                var dsRequest = finPrograms.ToDataSourceResult(request);
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set("FinProgramsCache", finPrograms, cacheOptions);
                return Json(dsRequest);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "An error occurred while fetching finprograms.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching financial programs. Please try again later.");
            }
        }
        public ActionResult FetchFinprogramsDropdown()
        {
            try
            {
                if (_memoryCache.TryGetValue("FinProgramsCache", out IEnumerable<FinPrograms> cachedData))
                {
                    return Json(cachedData);
                }
                IEnumerable<FinPrograms> finPrograms = _applicationFacade.FetchFinPrograms().Result;
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set("FinProgramsCache", finPrograms, cacheOptions);
                return Json(finPrograms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching FinprogramsDropdown.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching FinprogramsDropdown. Please try again later.");
            }
        }

        [Breadcrumb(FromAction = "GetFinPrograms", Title = "Franchisee")]
        public IActionResult GetFinProgramsFranchisee(int ProgramID)
        {
            if (ProgramID == 0)
            {
                var prevProgramId = TempData["ProgramID"];
                TempData["ProgramID"] = prevProgramId;
                TempData.Keep();
            }
            else
            {
                TempData["ProgramID"] = ProgramID;
                TempData.Keep();
            }
            _logger.LogInformation($"GetFinProgramsFranchisee initiated by user - " + User.Identity?.Name);
            return View();

        }

        public ActionResult FetchFinProgramsFranchisee([DataSourceRequest] DataSourceRequest request, int ProgramID, int PN)
        {
            if (PN > 1 && Convert.ToInt32(TempData["page"]) != PN)
            {
                request.Page = PN;
                TempData["page"] = PN;
                TempData.Keep();

            }
            try 
            {
                TempData["ProgramID"] = ProgramID;
                TempData.Keep();
                var cacheKey = $"FinProgramsFranchiseeCache_{ProgramID}";
                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FinProgramsFranchisee> cachedData))
                {
                    var dsRequestCache = cachedData.ToDataSourceResult(request);
                    return Json(dsRequestCache);
                }

                IEnumerable<FinProgramsFranchisee> finProgramsFranchise = _applicationFacade.FetchFinProgramsFranchisee(ProgramID).Result;
                var dsRequest = finProgramsFranchise.ToDataSourceResult(request);
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set(cacheKey, finProgramsFranchise, cacheOptions);
                return Json(dsRequest);
            }
           
             catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching FinProgramsFranchisee.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching FinProgramsFranchisee. Please try again later.");
            }
        }

        [Breadcrumb(FromAction = "GetFinProgramsFranchisee" ,Title="Invoice")]
        public IActionResult GetStatementsInvoice(int ProgramID, int MasterID, int PN)
        {
            TempData["page"] = PN;

            _logger.LogInformation($"GetStatementsInvoice initiated by user - " + User.Identity?.Name);
            TempData["FranchiseeProgramID"] = ProgramID;
          
            TempData["FranchiseeMasterID"] = MasterID;
            return View();
        }

  
        public ActionResult FetchInvoiceDetails([DataSourceRequest] DataSourceRequest request, int ProgramID, string MasterId)
        {
            try
            {
                var cacheKey = $"InvoiceDetailsCache_{ProgramID},{MasterId}";
                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<InvoiceDetails> cachedData))
                {
                    var dsRequestCache = cachedData.ToDataSourceResult(request);
                    return Json(dsRequestCache);
                }

                IEnumerable<InvoiceDetails> inoviceDetails = _applicationFacade.FetchInvoiceDetails(ProgramID, MasterId).Result;
                var dsRequest = inoviceDetails.ToDataSourceResult(request);
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set(cacheKey, inoviceDetails, cacheOptions);
                return Json(dsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching InvoiceDetails.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching InvoiceDetails. Please try again later.");
            }
        }
        

        public ActionResult FetchStatementDetails([DataSourceRequest] DataSourceRequest request, int ProgramID, string MasterId)
        {
            try
            {
                var cacheKey = $"InvoiceDetailsCache_{ProgramID},{MasterId}";
                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<StatementDetails> cachedData))
                {
                    var dsRequestCache = cachedData.ToDataSourceResult(request);
                    return Json(dsRequestCache);
                }

                IEnumerable<StatementDetails> statementDetails = _applicationFacade.FetchStatementDetails(ProgramID, MasterId).Result;
                var dsRequest = statementDetails.ToDataSourceResult(request);
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set(cacheKey, statementDetails, cacheOptions);
                return Json(dsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching StatementDetails.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching StatementDetails. Please try again later.");
            }
        }
        public IActionResult OpenPDF(string FilePath)
        {

            byte[] fileBytes;
            if (System.IO.File.Exists(FilePath))
            {
                fileBytes = System.IO.File.ReadAllBytes(FilePath);
            }
            else
            {
                _logger.LogInformation("File not found, accessed by user - " + User.Identity?.Name);
                return NotFound();
            }
            _logger.LogInformation($"{FilePath} - file is opened by user - " + User.Identity?.Name);

            return File(fileBytes, "application/pdf");
        }

      public IActionResult ShowAllfields(Boolean checkedOrNot) {
            
            TempData["showfields"] = checkedOrNot;
            return RedirectToAction("GetStatementsInvoice");
        }
        public IEnumerable<StatementDetails> FetchAllStatement(int ProgramID, string MasterID)
        {
            IEnumerable<StatementDetails> statementDetails = _applicationFacade.FetchStatementDetails(ProgramID, MasterID).Result;
            return statementDetails;
        }
        public IEnumerable<InvoiceDetails> FetchAllInvoice(int ProgramID, string MasterID)
        {
            IEnumerable<InvoiceDetails> InvoiceDetails = _applicationFacade.FetchInvoiceDetails(ProgramID, MasterID).Result;
            return InvoiceDetails;
        }
        public IEnumerable<FinPrograms> FetchAllFinPrograms()
        {
            IEnumerable<FinPrograms> finPrograms = _applicationFacade.FetchProgramMaintenanceFinPrograms().Result;
            return finPrograms;
        }
        public IEnumerable<FinProgramsFranchisee> FetchAllFranchiseeList(int ProgramID)
        {
            IEnumerable<FinProgramsFranchisee> finProgramsFranchise = _applicationFacade.FetchFinProgramsFranchisee(ProgramID).Result;
            return finProgramsFranchise;
        }
    }
}
