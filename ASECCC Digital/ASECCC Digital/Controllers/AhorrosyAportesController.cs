using ASECCC_Digital.Database;
using ASECCC_Digital.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class AhorrosyAportesController : Controller
    {
        private ASECCC_DIGITALEntities db = new ASECCC_DIGITALEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "AhorroyAporte";
        }

        // VISTAS

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

        public ActionResult ConsultarAportesAsociado()
        {
            return View();
        }


        // Método para crear un nuevo ahorro


        [HttpPost]
        public JsonResult CrearAhorro(string TipoAhorro, decimal Monto, int Plazo)
        {
            try
            {
                int usuarioId = 1; // Usuario forzado para pruebas

                var nuevoAhorro = new ASECCC_Digital.Database.Ahorros
                {
                    usuarioId = usuarioId,
                    tipoAhorroId = db.CatalogoTipoAhorro.FirstOrDefault(t => t.tipoAhorro == TipoAhorro)?.tipoAhorroId ?? 0,
                    montoActual = Monto,
                    fechaInicio = DateTime.Now,
                    plazo = Plazo,
                    estado = "activo"
                };

                db.Ahorros.Add(nuevoAhorro);
                db.SaveChanges();

                return Json(new { success = true, message = "Ahorro creado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al crear el ahorro: {ex.Message}" });
            }
        }

        // Metodo de Historial de Transacciones ahorro

        [HttpGet]
        public JsonResult ObtenerHistorialTransacciones(int ahorroId)
        {
            try
            {
                var transacciones = db.AhorroTransacciones
                    .Where(t => t.ahorroId == ahorroId)
                    .Select(t => new
                    {
                        Fecha = t.fechaTransaccion,
                        TipoTransaccion = t.tipoTransaccionId,
                        Monto = t.monto
                    })
                    .ToList();

                return Json(new { success = true, data = transacciones }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al obtener el historial: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }


        // Método para consultar los ahorros del usuario


        public ActionResult ConsultarAhorrosAsociado()
        {
            int usuarioId = 1; // Usuario forzado para pruebas

            var ahorros = db.Ahorros
                 .Include(a => a.CatalogoTipoAhorro)
                 .Where(a => a.usuarioId == usuarioId && a.estado == "activo")
                 .Select(a => new AhorroModel
                 {
                     AhorroId = a.ahorroId,
                     TipoAhorro = a.CatalogoTipoAhorro.tipoAhorro,
                     MontoActual = a.montoActual,
                     FechaInicio = a.fechaInicio,
                     Plazo = a.plazo,
                     Estado = a.estado
                 })
                 .ToList();

            return View(ahorros);
        }

        // Método para modificar el monto de un ahorro existente


        [HttpPost]
        public JsonResult ModificarMonto(int AhorroId, decimal NuevoMonto)
        {
            try
            {
                var ahorro = db.Ahorros.FirstOrDefault(a => a.ahorroId == AhorroId);
                if (ahorro == null)
                {
                    return Json(new { success = false, message = "Ahorro no encontrado." });
                }

                ahorro.montoActual = NuevoMonto;
                db.SaveChanges();

                return Json(new { success = true, message = "Monto modificado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al modificar el monto: {ex.Message}" });
            }
        }

        // Método para finalizar un ahorro


        [HttpPost]
        public JsonResult FinalizarAhorro(int ahorroId)
        {
            try
            {
                var ahorro = db.Ahorros.FirstOrDefault(a => a.ahorroId == ahorroId);
                if (ahorro == null)
                {
                    return Json(new { success = false, message = "Ahorro no encontrado." });
                }

                db.Ahorros.Remove(ahorro);
                db.SaveChanges();

                return Json(new { success = true, message = "Ahorro finalizado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al finalizar el ahorro: {ex.Message}" });
            }
        }
    }
}
