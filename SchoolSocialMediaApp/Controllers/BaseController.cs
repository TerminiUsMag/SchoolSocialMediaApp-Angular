using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SchoolSocialMediaApp.Controllers
{
    /// <summary>
    /// Base Controller for all controllers to inherit from
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        /// <summary>
        /// Returns the user id of the current user
        /// </summary>
        /// <returns></returns>
        protected string GetUserId()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }
    }
}
