using ASECCC_Digital.Models;
using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class AhorrosyAportesController : Controller
    {
        
        private readonly AhorroModel ahorroM = new AhorroModel();
        private readonly AporteModel aporteM = new AporteModel();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "AhorroyAporte";
        }


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

        [HttpGet]
        public JsonResult ConsultarAportesAsociado(string nombreAsociado)
        {
            var resultado = aporteM.ObtenerAportesPorAsociado(nombreAsociado);  
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RegistrarAportes()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegistrarAporte(string nombreAsociado, string tipoAporte, decimal monto)
        {
           var resultado = aporteM.RegistrarAporte(nombreAsociado, tipoAporte, monto);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModificarAportes()
        {
            return View();
        }   


        [HttpPost]
        public JsonResult ModificarAporte(int aporteId, decimal nuevoMonto)
        {
          var resultado = aporteM.ModificarAporte(aporteId, nuevoMonto);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarAporte(int aporteId)
        {
          var resultado = aporteM.EliminarAporte(aporteId);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ConsultarAhorrosAsociado(string nombreAsociado)
        {
          var resultado = ahorroM.ConsultarAhorrosAsociados(nombreAsociado);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarAhorro(string nombreAsociado, string tipoAhorro, decimal monto, int plazo)
        {
            var resultado = ahorroM.RegistrarAhorro(nombreAsociado, tipoAhorro, monto, plazo);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ModificarAhorro(int ahorroId, decimal nuevoMonto)
        {
          var resultado = ahorroM.ModificarAhorro(ahorroId, nuevoMonto);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

  
        [HttpPost]
        public JsonResult EliminarAhorro(int ahorroId)
        {
          var resultado = ahorroM.EliminarAhorro(ahorroId);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}
