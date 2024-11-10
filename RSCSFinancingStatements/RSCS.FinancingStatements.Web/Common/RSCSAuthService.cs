using Rscs.SecureApi.Core.Client.Interfaces;
using Rscs.SecureApi.Core.Client.Responses;
using Rscs.SecureApi.Core.Client.Services;

namespace RSCS.FinancingStatements.Web.Common
{
    public interface IRSCSAuthService
    {
        Task<bool> IsUserAuthorized(string userId, string systemId);
    }
    public class RSCSAuthService : IRSCSAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpApiClient _httpApiClient;
        private readonly ILogger<RSCSAuthService> _logger;
        public RSCSAuthService(IConfiguration configuration, IHttpApiClient httpApiClient, ILogger<RSCSAuthService> logger)
        {
            _configuration = configuration;
            _httpApiClient = httpApiClient;
            _logger = logger;
        }
        public async Task<bool> IsUserAuthorized(string userId, string systemId)
        {
            try
            {
                var apiKeyClient = new RscsSecureApiKeyService(_configuration, _httpApiClient);
                SecureApiAuthorizedResponse secureApiAuthorizedResponse = await apiKeyClient.IsUserAuthorizedAsync(userId, systemId);
                if (secureApiAuthorizedResponse.IsAuthorized)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
           
        }
    }

    
}
