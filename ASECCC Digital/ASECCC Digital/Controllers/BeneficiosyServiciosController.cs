using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class BeneficiosyServiciosController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "BenefyServ";
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