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

<<<<<<< HEAD
        public ActionResult Editar()
        {
            return View();
        }

        public ActionResult Eliminar()
=======
        public ActionResult Actualizar()
>>>>>>> 5983544f5d60eb677a6a608c80ea1883d3a72cc6
        {
            return View();
        }

        public ActionResult Buscar()
        {
            return View();
        }
<<<<<<< HEAD
=======

        public ActionResult Liquidar()
        {
            return View();
        }

        //--------VISTAS USUARIO--------------//

>>>>>>> 5983544f5d60eb677a6a608c80ea1883d3a72cc6
    }
}