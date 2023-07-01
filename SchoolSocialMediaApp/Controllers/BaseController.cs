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
