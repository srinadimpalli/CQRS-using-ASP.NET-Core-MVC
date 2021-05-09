using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreFactory.CQRS.Core
{
    // Sets active admin menu

    public class AdminMenuAttribute : ActionFilterAttribute
    {
        private string _menu { get; set; }

        public AdminMenuAttribute(string menu)
        {
            _menu = menu;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            (filterContext.Controller as Controller).ViewBag.AdminMenu = _menu;

            base.OnActionExecuting(filterContext);
        }
    }
}
