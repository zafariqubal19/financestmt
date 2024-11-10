using Microsoft.AspNetCore.Mvc;
using Rscs.SecureApi.Core.Client.Extensions;
using Rscs.SecureApi.Core.Client.Interfaces;
using Rscs.SecureApi.Core.Client.Models;
using Rscs.SecureApi.Core.Client.Requests;
using Rscs.SecureApi.Core.Client.Services;
using Rscs.SecureApi.Core.Client.Responses;
using RSCS.FinancingStatements.Web.Models;
using System.Security.Claims;
using System;

namespace RSCS.FinancingStatements.Web.Controllers
{
    // Controllers/AccountController.cs
    public class AccountController : Controller
    {
        
        private readonly IConfiguration _configuration;
        private readonly IHttpApiClient _httpApiClient;
        private readonly int _systemid;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IConfiguration configuration, IHttpApiClient httpApiClient, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _httpApiClient = httpApiClient;
            _systemid = int.Parse(_configuration["SystemID"]);
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try 
            {
               
                if (await IsValidUser(username, password))
                {
                    return RedirectToAction("getFinPrograms", "Invoice");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred during login: {ex}");
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
        }

        
        private async Task<bool> IsValidUser(string userId, string password)
        {
            try
            {
                if (User.Identity.IsAuthenticated) 
                {
                    if (userId != User.Identity.Name) 
                    {
                        _logger.LogError($"Windows user and login user is not matched..");
                        ViewBag.ErrorMessage = "Please enter windows username & password.";
                        return false;
                    }
                    var client = new RscsSecureApiService(_configuration, _httpApiClient);
                    var loginRequest = new LoginRequest()
                    {
                        SystemId = _systemid,
                        Password = password,
                        UserName = userId
                    };
                    var asyncLoginResult = await client.LoginAsync(loginRequest);
                    SecureApiJwtClaims secureApiJwtClaims = asyncLoginResult.ReadSecureApiToken();
                    TempData["Username"] = secureApiJwtClaims.UniqueName;
                    return secureApiJwtClaims != null;
                }
                else
                {
                    _logger.LogError($"User is not authenticated.");
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while validating user: {ex}");
                ViewBag.ErrorMessage = "Error occurred while validating user.";
                return false;
            }
        }
    }

}
