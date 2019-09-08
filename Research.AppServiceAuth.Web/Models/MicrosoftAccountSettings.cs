using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Research.AppServiceAuth.Web.Models
{
    public class MicrosoftAccountSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUriSuffix { get; set; } = ".auth/login/microsoftaccount/callback";
        public string GraphScope { get; set; } = "https://graph.microsoft.com/.default";
        public string RefreshTokenHeaderKey { get; set; } = "X-MS-TOKEN-MICROSOFTACCOUNT-REFRESH-TOKEN";
        public string OAuth2TokenUrl { get; set; } = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

        public string GetUsersInfoUrl(string userid)
        {
            return $"https://graph.microsoft.com/v1.0/users/{userid}";
        }
    }
}
