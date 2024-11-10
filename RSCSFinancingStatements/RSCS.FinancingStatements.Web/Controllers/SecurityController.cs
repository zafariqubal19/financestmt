using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using RSCS.FinancingStatements.Web.Facade;
using RSCS.FinancingStatements.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using SmartBreadcrumbs.Attributes;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace RSCS.FinancingStatements.Web.Controllers
{

    public class SecurityController : Controller
    {
        private readonly IApplicationFacade _applicationFacade;
        private readonly ILogger<SecurityController> _logger;
        private readonly IMemoryCache _memoryCache;

        public SecurityController(ILogger<SecurityController> logger, IApplicationFacade applicationFacade, IMemoryCache memoryCache)
        {
            _logger = logger;
            _applicationFacade = applicationFacade;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProgramFranchiseeUser()
        {
            _logger.LogInformation($"GetFinProgramsFranchisee initiated by user - " + User.Identity?.Name);
            return View();
        }
        public ActionResult FetchFinProgFranchisee([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                if (_memoryCache.TryGetValue("FinProgFranchiseeCache", out IEnumerable<References> cachedData))
                {
                    var dsRequestCatche = cachedData.ToDataSourceResult(request);
                    return Json(dsRequestCatche);
                }
                IEnumerable<References> FinProgFranchisee = _applicationFacade.FetchSecurityFinProgFranchisee().Result;
                var dsRequest = FinProgFranchisee.ToDataSourceResult(request);
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set("FinProgFranchiseeCache", FinProgFranchisee, cacheOptions);
                return Json(dsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Security FinProgFranchisee.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching Security FinProgFranchisee. Please try again later.");
            }
        }

        public ActionResult FetchSecurtiyContacts([DataSourceRequest] DataSourceRequest request, int MasterID)
       {
            request.Filters.ToString();
            try
            {
                IEnumerable<Contacts> contacts = _applicationFacade.FetchSecurityContacts(MasterID).Result;
                var dsRequest = contacts.ToDataSourceResult(request);
                return Json(dsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Security FetchSecurtiyContacts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching Security FetchSecurtiyContacts. Please try again later.");
            }

        }
        [HttpPost]
        public async Task<string> GrantOrRestrictAccess([FromBody] List<AccesResquestParameter> accesResquestParameter)
        {
            int effedtedRow = 0;
            try
            {
                foreach (var accesResquestEntity in accesResquestParameter)
                {
                    AccesResquestParameter accesResquest = new AccesResquestParameter();
                    accesResquest.NameID = accesResquestEntity.NameID;
                    accesResquest.ResourceSecurityValue = accesResquestEntity.ResourceSecurityValue;
                    accesResquest.ResourceSecurityTypeID = accesResquestEntity.ResourceSecurityTypeID;
                    accesResquest.LastModifiedBy = User.Identity?.Name;
                    accesResquest.Contact = accesResquestEntity.Contact;

                    effedtedRow = await _applicationFacade.GrantOrRestrictAccess(accesResquest);
                }
                if (effedtedRow > 0)
                {
                    _logger.LogInformation($"GrantOrRestrictAccess status is updated by user - " + User.Identity?.Name);
                    return "Access status Updated";
                }
                else
                {
                    _logger.LogInformation($"GrantOrRestrictAccess status not updated..");
                    return "Access Status Not updated";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while granting or restricting access:." + ex.Message);
                return "An error occurred while granting or restricting access: " + ex.Message;
            }

        }

        public IEnumerable<Contacts> FetchAllSecurtiyContacts( int MasterID)
        {
            IEnumerable<Contacts> contacts = _applicationFacade.FetchSecurityContacts(MasterID).Result;
            return contacts;
        }
    }
}
