using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASECCC_Digital.Controllers
{
    public class AsociadosController : Controller
    {
        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Asociados"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }
        //--------VISTAS ADMIN--------------//

        // GET: Asociados
        public ActionResult Asociados()
        {
            return View();
        }


        public ActionResult RegistrarAsociado()
        {
            return View();
        }


        public ActionResult ActualizarAsociado()
        {
            return View();
        }

        public ActionResult BuscarAsociado()
        {
            return View();
        }

        public ActionResult LiquidarAsociado()
        {
            return View();
        }

        //--------VISTAS USUARIO--------------//


    }
}