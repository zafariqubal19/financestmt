using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSCS.FinancingStatements.Web.Common;
using RSCS.FinancingStatements.Web.Facade;
using RSCS.FinancingStatements.Web.Models;

namespace RSCS.FinancingStatements.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly string? _systemid;
        private readonly IConfiguration _configuration;
        public IRSCSAuthService _rscsAuthService;
        private readonly IApplicationFacade _applicationFacade;
        private readonly ILogger<AuthorizationMiddleware> _logger;
        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration, IRSCSAuthService rscsAuthService, IApplicationFacade applicationFacade, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _systemid = _configuration["SystemID"];
            _rscsAuthService = rscsAuthService;
            _applicationFacade = applicationFacade;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
           
            try
            {
                var accessToken = _applicationFacade.GetAccessToken();
                string userId = GetUserId(context.User.Identity.Name);
                userId = "MSingh";
                if (!string.IsNullOrEmpty(accessToken.AccessToken))
                {
                    if (await IsAuthorized(userId, _systemid))
                    {
                        _applicationFacade.SetBearerToken(accessToken.AccessToken);
                        var claims = new[]
                        {
                                new Claim(ClaimTypes.NameIdentifier, userId),
                                new Claim(ClaimTypes.Name, userId)
                            };

                        var identity = new ClaimsIdentity(claims, "CustomAuth");
                        var principal = new ClaimsPrincipal(identity);
                        context.User = principal;
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("You are not authorized, please contact RSCS administrator.");
                        return;
                    }
                }
                else
                {
                    _logger.LogError("Access token is null or empty.");
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized. Access token is missing.");
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AuthorizationMiddleware.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred while processing your request.");
            }
        }

        private async Task<bool> IsAuthorized(string userId, string systemId)
        {
            //return await _rscsAuthService.IsUserAuthorized(userId, systemId);
            return true;
        }
        private string GetUserId(string fullUserId)
        {
            int index = fullUserId.IndexOf('\\');
            return (index != -1 && index < fullUserId.Length - 1) ? fullUserId.Substring(index + 1) : fullUserId;
        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }

}
