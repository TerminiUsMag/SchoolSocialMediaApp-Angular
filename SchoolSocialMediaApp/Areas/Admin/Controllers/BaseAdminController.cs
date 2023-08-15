using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SchoolSocialMediaApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Base Admin Controller for all controllers in the Admin area to inherit from
    /// </summary>
    [Authorize]
    public class BaseAdminController : Controller
    {
        /// <summary>
        /// Returns the user id of the current user
        /// </summary>
        /// <returns></returns>
        protected Guid GetUserId()
        {
            var id = Guid.Empty;

            if (User != null)
            {
                if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid result))
                {
                    id = result;
                }

            }

            return id;
        }
    }
}
