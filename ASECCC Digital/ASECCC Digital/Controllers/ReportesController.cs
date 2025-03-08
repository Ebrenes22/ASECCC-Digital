using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class ReportesController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "Reportes";
        }

        //--------VISTAS ADMIN--------------//
        // GET: Reportes
        public ActionResult Reporte()
        {
            return View();
        }

        public ActionResult EstadodeCuentaAsociados()
        {
            return View();
        }



        //--------VISTAS USUARIO-------------//

        public ActionResult GenerarEstadoAsociado()
        {
            return View();
        }

    }
}