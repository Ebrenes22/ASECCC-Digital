using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class SeguridadyPermisosController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "SeguridadyPermisos"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }


        //--------VISTAS ADMIN--------------//
        // GET: SeguridadyPermisos

        public ActionResult SeguridadyPermiso()
        {
            return View();
        }
        public ActionResult RolesyPermisos()
        {
            return View();
        }

        public ActionResult RegistroActividadAuditoria()
        {
            return View();
        }




        //--------VISTAS USUARIO-------------//
    }
}