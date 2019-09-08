using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Research.AppServiceAuth.Web.Models
{
    public partial class MsLiveProfile
    {
        
        
    }

    public partial class MsLiveProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }

        [JsonProperty("gender")]
        public object Gender { get; set; }

        [JsonProperty("emails")]
        public Emails Emails { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("updated_time")]
        public object UpdatedTime { get; set; }

        public GraphProfile ToGraphProfile()
        {
            return new GraphProfile
            {
                Id = Id,
                DisplayName = Name,
                GivenName = FirstName,
                Surname = LastName,
                UserPrincipalName = Emails.Account,
                Mail = Emails.Preferred
            };
        }
    }

    public partial class Emails
    {
        [JsonProperty("preferred")]
        public string Preferred { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("personal")]
        public object Personal { get; set; }

        [JsonProperty("business")]
        public object Business { get; set; }
    }
}
