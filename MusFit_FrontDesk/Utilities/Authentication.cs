using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace MusFit_FrontDesk.Utilities
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("SAccount") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "Controller", "Front" },
                    { "Action", "Login" }
                });
            }
        }
    }
}
