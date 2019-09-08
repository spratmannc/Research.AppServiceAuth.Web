using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Research.AppServiceAuth.Web.Models
{
    public partial class AadTokenPayload
    {
        [JsonProperty("aud")]
        public Guid Aud { get; set; }

        [JsonProperty("iss")]
        public Uri Iss { get; set; }

        [JsonProperty("iat")]
        public long Iat { get; set; }

        [JsonProperty("nbf")]
        public long Nbf { get; set; }

        [JsonProperty("exp")]
        public long Exp { get; set; }

        [JsonProperty("aio")]
        public string Aio { get; set; }

        [JsonProperty("amr")]
        public string[] Amr { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("ipaddr")]
        public string Ipaddr { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("oid")]
        public Guid Oid { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("tid")]
        public Guid Tid { get; set; }

        [JsonProperty("unique_name")]
        public string UniqueName { get; set; }

        [JsonProperty("upn")]
        public string Upn { get; set; }

        [JsonProperty("uti")]
        public string Uti { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }

        public GraphProfile ToGraphProfile()
        {
            return new GraphProfile
            {
                Id = Oid.ToString(),
                DisplayName = Name,
                GivenName = GivenName,
                UserPrincipalName = Upn,
                Surname = FamilyName
            };
        }
    }
}
