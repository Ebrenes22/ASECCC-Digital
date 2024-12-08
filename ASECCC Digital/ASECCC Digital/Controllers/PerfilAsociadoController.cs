using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{


    public class PerfilAsociadoController : Controller
    {


        // Acción que se ejecuta antes de cada acción del controlador   
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Asociados"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }


        //--------VISTAS USUARIOS-------------//

        // GET: Prestamos
        public ActionResult PerfilAsociado()
        {
            return View();
        }

        public ActionResult BeneficiariosAsociado()
        {
            return View();
        }



    }
}
