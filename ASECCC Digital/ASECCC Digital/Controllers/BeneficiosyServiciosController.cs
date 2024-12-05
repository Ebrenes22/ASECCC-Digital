using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class BeneficiosyServiciosController : Controller
    {
        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "BenefyServ"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }


        //--------VISTAS ADMIN--------------//
        // GET: BeneficiosyServicios
        public ActionResult BeneficioyServicio()
        {
            return View();
        }

        public ActionResult GestionarBenefyServ()
        {
            return View();
        }

        public ActionResult ConsultarBenefyServAdmin()
        {
            return View();
        }

        public ActionResult RegistrarCuentaxCobrar()
        {
            return View();
        }

        public ActionResult RegistrarAbonoBenefyServ()
        {
            return View();
        }

        //--------VISTAS ASOCIADOS--------------//

        public ActionResult BenefyServDisponibles()
        {
            return View();
        }

        public ActionResult ConsultarBenefyServAsociado()
        {
            return View();
        }


    }
}