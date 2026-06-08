using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class AhorrosyAportesController : Controller
    {
        private ASECCC_DIGITALEntities db = new ASECCC_DIGITALEntities();
        private readonly AhorroModel ahorroModel = new AhorroModel();
        private readonly AporteModel aporteModel = new AporteModel();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "AhorroyAporte";
        }

        // Vistas
        public ActionResult AhorroyAporte() => View();

        [HttpGet]
        [Authorize]
        public ActionResult ConsultarAhorrosAsociado()
        {
            return View();
        }

        //
        // A H O R R O S
        //

        [HttpPost]
        public JsonResult AgregarAbonoAhorro(int ahorroId, decimal monto, string descripcion = null)
        {
            try
            {
                var resultado = ahorroModel.AgregarAbonoAhorro(ahorroId, monto, descripcion);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al agregar el abono: " + ex.Message });
            }
        }


        [HttpGet]
        public JsonResult ObtenerAhorrosAsociado()
        {
            try
            {
                int usuarioId = (int)Session["usuarioId"];
                var lista = ahorroModel.ObtenerAhorrosPorAsociado(usuarioId);

                var data = lista.Select(a => new
                {
                    AhorroId = a.ahorroId,
                    TipoAhorro = a.CatalogoTipoAhorro.tipoAhorro,
                    MontoInicial = a.montoInicial,
                    MontoActual = a.montoActual,
                    FechaInicio = a.fechaInicio,
                    FechaFin = a.fechaFin,
                    Plazo = a.plazo,
                    Estado = a.estado
                }).ToList();

                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al consultar los ahorros: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RegistrarAhorro(string tipoAhorro, decimal monto, int plazo)
        {
            try
            {
                int usuarioId = (int)Session["usuarioId"];
                var resultado = ahorroModel.RegistrarAhorroPorId(usuarioId, tipoAhorro, monto, plazo);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar el ahorro: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ModificarAhorro(int ahorroId, decimal nuevoMonto)
        {
            try
            {
                var resultado = ahorroModel.ModificarAhorro(ahorroId, nuevoMonto);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al modificar el ahorro: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarAhorro(int ahorroId)
        {
            try
            {
                var resultado = ahorroModel.EliminarAhorro(ahorroId);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el ahorro: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult FinalizarAhorro(int ahorroId)
        {
            try
            {
                var resultado = ahorroModel.FinalizarAhorro(ahorroId);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al finalizar el ahorro: " + ex.Message });
            }
        }


        [HttpGet]
        public JsonResult ObtenerHistorialTransacciones(int ahorroId)
        {
            try
            {
                var resultado = ahorroModel.ObtenerHistorialAhorro(ahorroId);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener el historial: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SolicitarRetiro(int ahorroId, decimal montoRetiro)
        {
            int usuarioId = Convert.ToInt32(Session["UsuarioId"]);

            var model = new AhorroModel();

            bool resultado = model.SolicitarRetiro(
                ahorroId,
                montoRetiro,
                usuarioId);

            return Json(new
            {
                success = resultado,
                message = resultado
                    ? "Solicitud enviada correctamente."
                    : "No fue posible registrar la solicitud."
            });
        }


        //
        // A P O R T E S 
        //

        [HttpGet]
        [Authorize]
        public ActionResult ConsultarAportesAsociado()
        {
            int usuarioId = (int)Session["usuarioId"];
            var resultado = aporteModel.AportesPorAsociado(usuarioId);

            if ((bool)resultado.GetType().GetProperty("success").GetValue(resultado))
            {
                var data = resultado.GetType().GetProperty("data").GetValue(resultado) as List<Aporte>;
                return View(data);
            }

            ViewBag.Mensaje = resultado.GetType().GetProperty("message").GetValue(resultado);
            return View(new List<Aporte>());
        }


        [HttpGet]
        public JsonResult ObtenerHistorialAporte(int aporteId)
        {
            try
            {
                var resultado = aporteModel.ObtenerHistorialAporte(aporteId);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener el historial del aporte: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RegistrarAporte(string tipoAporte, decimal monto)
        {
            try
            {
                int usuarioId = (int)Session["usuarioId"];
                var resultado = aporteModel.RegistrarAporte(usuarioId, tipoAporte, monto);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar el aporte: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AgregarAporte(int aporteId, decimal monto, string descripcion = null)
        {
            try
            {
                var resultado = aporteModel.AgregarAporte(aporteId, monto, descripcion);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al agregar el aporte: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult ModificarAporte(int aporteId, decimal nuevoMonto)
        {
            try
            {
                var resultado = aporteModel.ModificarAporte(aporteId, nuevoMonto);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al modificar el aporte: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarAporte(int aporteId)
        {
            try
            {
                var resultado = aporteModel.EliminarAporte(aporteId);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el aporte: " + ex.Message });
            }
        }

        //Admin

        [HttpGet]
        [Authorize]
        public ActionResult GestionarAhorrosAdmin()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ConsultarAhorrosPorNombre(string nombreAsociado)
        {
            var resultado = ahorroModel.ConsultarAhorrosAsociado(nombreAsociado);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ConsultarAportesPorNombre(string nombreAsociado)
        {
            var resultado = aporteModel.ConsultarAportesPorNombre(nombreAsociado);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GestionarAportes()
        {
            return View();
        }


        [HttpPost]
        public JsonResult RegistrarAportePorNombre(string nombreAsociado, string tipoAporte, decimal monto)
        {
            try
            {
                var usuario = db.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return Json(new { success = false, message = "Asociado no encontrado." });

                var resultado = aporteModel.RegistrarAporte(usuario.usuarioId, tipoAporte, monto);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar el aporte: " + ex.Message });
            }
        }


        [HttpGet]
        public JsonResult BuscarAsociados(string termino)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var asociados = context.Usuario
                    .Where(u => u.nombreCompleto.Contains(termino))
                    .Select(u => new
                    {
                        label = u.nombreCompleto,
                        value = u.usuarioId
                    }).ToList();

                return Json(asociados, JsonRequestBehavior.AllowGet);
            }
        }


    }
}