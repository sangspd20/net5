using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace F88.Digital.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class DeactivatedModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}