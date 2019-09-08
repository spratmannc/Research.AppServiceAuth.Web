using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research.AppServiceAuth.Web.Models;
using Research.AppServiceAuth.Web.Services;

namespace Research.AppServiceAuth.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserInfoService userInfo;

        public IndexModel(UserInfoService userInfo)
        {
            this.userInfo = userInfo;
        }

        [BindProperty]
        public GraphProfile UserDetails { get; private set; }

        [BindProperty]
        public string LastJson { get; private set; }

        public async Task OnGet()
        {
            UserDetails = await userInfo.GetCurrentProfile();

            LastJson = userInfo.LastJson;
        }
    }
}
