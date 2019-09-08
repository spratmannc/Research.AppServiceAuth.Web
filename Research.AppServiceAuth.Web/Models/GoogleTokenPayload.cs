using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Research.AppServiceAuth.Web.Models
{
    public partial class GoogleTokenPayload
    {
        [JsonProperty("iss")]
        public Uri Iss { get; set; }

        [JsonProperty("azp")]
        public string Azp { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("at_hash")]
        public string AtHash { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public Uri Picture { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("iat")]
        public long Iat { get; set; }

        [JsonProperty("exp")]
        public long Exp { get; set; }

        public GraphProfile ToGraphProfile()
        {
            return new GraphProfile
            {
                Id = Sub,
                DisplayName = Name,
                GivenName = GivenName,
                Surname = FamilyName,
                Mail = Email,
                UserPrincipalName = Email
            };
        }
    }
}
