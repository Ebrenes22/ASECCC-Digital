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
            using (var db = new ASECCC_DIGITALEntities())
            {
                var aportes = db.Aportes
                    .Where(a => a.usuarioId != null)
                    .Select(a => new AporteModel
                    {
                        AporteId = a.aporteId,
                        TipoAporte = a.tipoAporte,
                        Monto = a.monto,
                        FechaRegistro = a.fechaRegistro ?? DateTime.MinValue,
                    })
                    .ToList();

                return View(aportes);
            }
        }


        // Consultar aportes de un asociado
        [HttpGet]
        public JsonResult ConsultarAportesAsociado(string nombreAsociado)
        {
            try
            {
                var usuario = db.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return Json(new { success = false, message = "Asociado no encontrado." }, JsonRequestBehavior.AllowGet);

                var aportes = db.Aportes
                    .Where(a => a.usuarioId == usuario.usuarioId)
                    .Select(a => new
                    {
                        AporteId = a.aporteId,
                        TipoAporte = a.tipoAporte,
                        Monto = a.monto,
                        FechaRegistro = a.fechaRegistro,
                        Estado = "Activo"
                    })
                    .ToList();

                return Json(new { success = true, data = aportes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al consultar los aportes." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RegistrarAhorro(string nombreAsociado, string tipoAhorro, decimal monto, int plazo)
        {
            try
            {
                var usuario = db.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return Json(new { success = false, message = "Asociado no encontrado." });

                var tipoAhorroId = db.CatalogoTipoAhorro.FirstOrDefault(t => t.tipoAhorro == tipoAhorro)?.tipoAhorroId ?? 0;
                if (tipoAhorroId == 0)
                    return Json(new { success = false, message = "Tipo de ahorro no válido." });

                var nuevoAhorro = new Ahorros
                {
                    usuarioId = usuario.usuarioId,
                    tipoAhorroId = tipoAhorroId,
                    montoInicial = monto,
                    montoActual = monto,
                    fechaInicio = DateTime.Now,
                    plazo = plazo,
                    estado = "activo"
                };

                db.Ahorros.Add(nuevoAhorro);
                db.SaveChanges();

                return Json(new { success = true, message = "Ahorro registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar el ahorro: " + ex.Message });
            }
        }




        // Método para crear un nuevo ahorro


        [HttpPost]
        public JsonResult CrearAhorro(string TipoAhorro, decimal Monto, int Plazo)
        {
            try
            {
                int usuarioId = 1; // Usuario  para pruebas

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


        [HttpGet]
        public JsonResult ConsultarAhorrosAsociado(string nombreAsociado)
        {
            try
            {
                // Buscar el usuario por su nombre
                var usuario = db.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Asociado no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                // Obtener ahorros del usuario
                var ahorros = db.Ahorros
                    .Where(a => a.usuarioId == usuario.usuarioId && a.estado == "activo")
                    .Select(a => new
                    {
                        AhorroId = a.ahorroId,
                        TipoAhorro = a.CatalogoTipoAhorro.tipoAhorro,
                        MontoActual = a.montoActual,
                        FechaInicio = a.fechaInicio,
                        Plazo = a.plazo,
                        Estado = a.estado
                    })
                    .ToList();

                return Json(new { success = true, data = ahorros }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al consultar los ahorros: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // Método para modificar el monto de un ahorro existente


        [HttpPost]
        public JsonResult ModificarAhorro(int ahorroId, decimal nuevoMonto)
        {
            try
            {
                var ahorro = db.Ahorros.Find(ahorroId);
                if (ahorro == null) return Json(new { success = false, message = "Ahorro no encontrado." });

                ahorro.montoActual = nuevoMonto;
                db.SaveChanges();

                return Json(new { success = true, message = "Monto del ahorro actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al modificar el ahorro: " + ex.Message });
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


        // Registrar un nuevo aporte
        [HttpPost]
        public JsonResult RegistrarAporte(string nombreAsociado, string tipoAporte, decimal monto)
        {
            try
            {
                var usuario = db.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return Json(new { success = false, message = "Asociado no encontrado." });

                var nuevoAporte = new Aportes
                {
                    usuarioId = usuario.usuarioId,
                    tipoAporte = tipoAporte,
                    monto = monto,
                    fechaRegistro = DateTime.Now
                };

                db.Aportes.Add(nuevoAporte);
                db.SaveChanges();

                return Json(new { success = true, message = "Aporte registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar el aporte." });
            }
        }


        // Modificar un aporte
        [HttpPost]
        public JsonResult ModificarAporte(int aporteId, decimal nuevoMonto)
        {
            try
            {
                var aporte = db.Aportes.Find(aporteId);
                if (aporte == null) return Json(new { success = false, message = "Aporte no encontrado." });

                aporte.monto = nuevoMonto;
                db.SaveChanges();

                return Json(new { success = true, message = "Monto del aporte actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al modificar el aporte." });
            }
        }


        [HttpPost]
        public JsonResult EliminarAhorro(int ahorroId)
        {
            try
            {
                var ahorro = db.Ahorros.Find(ahorroId);
                if (ahorro == null) return Json(new { success = false, message = "Ahorro no encontrado." });

                db.Ahorros.Remove(ahorro);
                db.SaveChanges();

                return Json(new { success = true, message = "Ahorro eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el ahorro: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarAporte(int aporteId)
        {
            try
            {
                var aporte = db.Aportes.FirstOrDefault(a => a.aporteId == aporteId);
                if (aporte == null) return Json(new { success = false, message = "Aporte no encontrado." });

                db.Aportes.Remove(aporte);
                db.SaveChanges();

                return Json(new { success = true, message = "Aporte eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar el aporte: {ex.Message}" });
            }
        }

    }
}
