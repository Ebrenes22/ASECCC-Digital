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
        public ActionResult Asociado()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }


        public ActionResult Actualizar()
        {
            return View();
        }

        public ActionResult Buscar()
        {
            return View();
        }

        public ActionResult Liquidar()
        {
            return View();
        }

        //--------VISTAS USUARIO--------------//


    }
}