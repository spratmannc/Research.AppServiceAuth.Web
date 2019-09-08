using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Research.AppServiceAuth.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Research.AppServiceAuth.Web.Services
{
    public class UserInfoService
    {
        private readonly HttpClient http;
        private readonly IHttpContextAccessor httpContext;
        private readonly IMemoryCache cache;
        private readonly MicrosoftAccountSettings settings;

        public UserInfoService(HttpClient http, IHttpContextAccessor httpContext, IMemoryCache cache, IOptions<MicrosoftAccountSettings> options)
        {
            this.http = http;
            this.httpContext = httpContext;
            this.cache = cache;
            this.settings = options.Value;
        }

        public async Task<GraphProfile> GetCurrentProfile()
        {
            var userid = GetHeaderValue("X-MS-CLIENT-PRINCIPAL-ID");
            var principalIDP = GetHeaderValue("X-MS-TOKEN-GOOGLE-ID-TOKEN");

            if (!string.IsNullOrEmpty(userid))
            {
                string cacheKey = BuildCacheKey(userid, principalIDP);

                if (cache.TryGetValue<GraphProfile>(cacheKey, out var profile))
                {
                    return profile;
                }
                else
                {
                    return await AcquireUserInfo(userid);
                }
            }
            else
            {
                return null;
            }
        }

        private string BuildCacheKey(string userid, string principalIDP)
        {
            return $"{principalIDP}:{userid}";
        }

        private async Task<GraphProfile> AcquireUserInfo(string userId)
        {
            string principalIDP = GetHeaderValue("X-MS-CLIENT-PRINCIPAL-IDP");

            switch (principalIDP)
            {
                case "microsoftaccount":
                    return await AcquireMicrosoftAccountUserInfo(userId);

                case "google":
                    return ParseGoogleIdToken();

                default:
                    return null;
            }

        }

        private string GetHeaderValue(string key)
        {
            var context = httpContext.HttpContext.Request;
            context.Headers.TryGetValue(key, out var values);
            return values.FirstOrDefault();
        }

        private GraphProfile ParseGoogleIdToken()
        {
            var jwt = GetHeaderValue("X-MS-TOKEN-GOOGLE-ID-TOKEN");
            var encodedPayload = jwt.Split('.')[1];
            var bits = Convert.FromBase64String(encodedPayload);
            var json = Encoding.UTF8.GetString(bits);
            var payload = JsonConvert.DeserializeObject<GoogleTokenPayload>(json);

            var profile = payload.ToGraphProfile();

            cache.Set(BuildCacheKey("google", profile.Id), profile);

            return profile;
        }

        private async Task<GraphProfile> AcquireMicrosoftAccountUserInfo(string userid)
        {
            if (!string.IsNullOrEmpty(userid))
            {
                var token = await GetAccessTokenViaRefresh();

                if (!string.IsNullOrEmpty(token))
                {
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await http.GetAsync(settings.GetUsersInfoUrl(userid));

                    var json = await response.Content.ReadAsStringAsync();

                    var profile = JsonConvert.DeserializeObject<GraphProfile>(json);

                    if (profile != null)
                    {
                        cache.Set(BuildCacheKey("microsoftaccount", profile.Id), profile);
                    }

                    return profile;
                }
            }

            return null;
        }

        private async Task<string> GetAccessTokenViaRefresh()
        {
            var context = httpContext.HttpContext.Request;

            var refresh_token = GetHeaderValue(settings.RefreshTokenHeaderKey);

            if (!string.IsNullOrEmpty(refresh_token))
            {
                var pairs = new KeyValuePair<string, string>[]
                {
                new KeyValuePair<string, string>("client_id", settings.ClientId),
                new KeyValuePair<string, string>("client_secret", settings.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", $"{context.Scheme}://{context.Host}/{settings.RedirectUriSuffix}"),
                new KeyValuePair<string, string>("scope", settings.GraphScope),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refresh_token)
                };

                var content = new FormUrlEncodedContent(pairs);

                var response = await http.PostAsync(settings.OAuth2TokenUrl, content);

                var json = await response.Content.ReadAsStringAsync();

                var token = JsonConvert.DeserializeObject<TokenResponse>(json);

                return token.AccessToken;
            }
            else
            {
                return null;
            }
        }
    }
}
