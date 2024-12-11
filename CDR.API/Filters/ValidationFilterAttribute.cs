using CDR.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CDR.API.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly UserManager<User> _userManager;

        public ValidationFilterAttribute(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _userManager.GetUserAsync(context.HttpContext.User).Result;

            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "Logout" }));
            }
            else
            {
                if (user.PackageFinishDate < DateTime.Now)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Page", action = "TimeIsUp" }));
                }
            }
        }
    }
}
