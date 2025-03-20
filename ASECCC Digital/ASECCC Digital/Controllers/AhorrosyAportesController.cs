using ASECCC_Digital.Models;
using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace ASECCC_Digital.Controllers
{
    public class AhorrosyAportesController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "AhorroyAporte";
        }

        //Instancias de los modelos Ahorro y Aporte
        private readonly AhorroModel ahorroM = new AhorroModel();
        private readonly AporteModel aporteM = new AporteModel();




        [Authorize]
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
        [Authorize]
        public JsonResult ConsultarAportesAsociado(string nombreAsociado)
        {
            var resultado = aporteM.ObtenerAportesPorAsociado(nombreAsociado);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarAporte(string nombreAsociado, string tipoAporte, decimal monto)
        {
            var resultado = aporteM.RegistrarAporte(nombreAsociado, tipoAporte, monto);
            return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public JsonResult BuscarAsociados(string termino)
        {
<<<<<<< Updated upstream
            using (var context = new ASECCC_DIGITALEntities())
=======
            var usuarioId = (int)Session["usuarioId"];
            var ahorros = ahorroM.ConsultarAhorrosAsociados(usuarioId).Data as dynamic; 

            if (ahorros != null && ahorros.success)
>>>>>>> Stashed changes
            {
                var asociados = context.Usuario
                    .Where(u => u.nombreCompleto.Contains(termino))
                    .Select(u => new { label = u.nombreCompleto, value = u.usuarioId })
                    .Take(10)
                    .ToList();

                return Json(asociados, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ConsultarAhorrosAsociado(string nombreAsociado)
        {
            var resultado = ahorroM.ConsultarAhorrosAsociado(nombreAsociado);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarAhorro(string nombreAsociado, string tipoAhorro, decimal monto, int plazo)
        {
            var resultado = ahorroM.RegistrarAhorro(nombreAsociado, tipoAhorro, monto, plazo);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerHistorialAporte(int aporteId)
        {
            var resultado = aporteM.ObtenerHistorialAporte(aporteId);
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
