using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = GetCurrentModule();
        }

        protected abstract string GetCurrentModule();
    }
}