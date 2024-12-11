using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class AhorrosyAportesController : Controller
    {
        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "AhorroyAporte"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }

        //--------VISTAS ADMIN--------------//
        // GET: AhorrosyAportes
        public ActionResult AhorroyAporte()
        {
            return View();
        }

        public ActionResult GestionarAportes()
        {
            return View();
        }

        public ActionResult GestionarAhorrosAdmin()
        {
            return View();
        }

      
        //--------VISTAS ASOCIADOS--------------//

        public ActionResult ConsultarAportesAsociado()
        {
            return View();
        }

        public ActionResult ConsultarAhorrosAsociado()
        {
            return View();
        }

        public ActionResult CrearAhorro()
        {
            return View();
        }

        public ActionResult ModificarAhorro()
        {
            return View();
        }

        public ActionResult FinalizarAhorro()
        {
            return View();
        }



    }
}