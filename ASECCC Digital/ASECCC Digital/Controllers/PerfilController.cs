using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{


    public class PerfilController : BaseController
    {

        protected override string GetCurrentModule()
        {
            return "Asociados";
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
