    using ASECCC_Digital.Models;
    using ASECCC_Digital.ViewModels;
    using ASECCC_Digital.Database;
    using ASECCC_Digital.Entities;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Web.Util;

    namespace ASECCC_Digital.Controllers
    {
        public class PrestamosController : BaseController
        {    

        protected override string GetCurrentModule()
        {
            return "Prestamos"; 
        }

        //Instancia del modelo prestamos
        private readonly PrestamosModel prestamoM = new PrestamosModel();


        #region Vistas ADMIN
        [Authorize]

        public ActionResult Prestamo()
            {
                // Llamar a un método de prestamoM para obtener los datos necesarios
                //var prestamos = prestamoM.ObtenerListaPrestamosAdmin();
                return View();  // Pasar el modelo a la vista
            }


        [HttpGet]
        public ActionResult RegistrarAbonos(string NombreCompleto)
        {
            List<string> sugerencias;
            var prestamos = prestamoM.ObtenerPrestamosPorUsuario(NombreCompleto, out sugerencias);

            if (prestamos.Count == 0 && sugerencias.Count > 0)
            {
                TempData["Sugerencias"] = sugerencias; // Pasar sugerencias a la vista
            }

            var viewModel = new PrestamoUsuarioTransaccionesViewModel
            {
                NombreCompleto = NombreCompleto,
                ListaPrestamos = prestamos
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult RegistrarAbonos(PrestamoTransaccionViewModel model)
        {
            if (model == null || model.MontoAbonado <= 0)
            {
                TempData["MensajeError"] = "El monto abonado debe ser mayor a 0.";
                return RedirectToAction("RegistrarAbonos");
            }

            bool resultado = prestamoM.RegistrarAbono(model);
            if (resultado)
            {
                TempData["Mensaje"] = "Abono registrado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "Error al registrar el abono.";
            }

            return RedirectToAction("RegistrarAbonos" );
        }




        [Authorize]
        public ActionResult ConsultaPrestamosAdmin()
        {
            // Llamar a un método en prestamoM para obtener los préstamos para consulta admin
            //var prestamos = prestamoM.ObtenerPrestamosParaConsultaAdmin();
            return View();
        }

        public ActionResult RevisionPrestamos()
        {
            var viewModel = new SolicitudPrestamoViewModel();

            // Aquí debes llenar las listas de solicitudes según el estado.
            // Por ejemplo:
            var model = new PrestamosModel();
            viewModel.Solicitudes.Pendientes = model.ObtenerSolicitudesPorEstado("Pendiente");
            viewModel.Solicitudes.EnRevision = model.ObtenerSolicitudesPorEstado("Revision");
            viewModel.Solicitudes.Aprobadas = model.ObtenerSolicitudesPorEstado("Aprobada");
            viewModel.Solicitudes.Rechazadas = model.ObtenerSolicitudesPorEstado("Rechazada");

            return View(viewModel);
        }

        // Acción que devuelve los detalles de una solicitud en formato JSON
        [HttpGet]
        public ActionResult ObtenerSolicitudPorId(int id)
        {
            var model = new PrestamosModel();
            var viewModel = model.ObtenerSolicitudPorId(id);
            if (viewModel != null && viewModel.DetalleSolicitud != null)
            {
                return Json(new
                {
                    viewModel.DetalleSolicitud.SolicitudPrestamoId,
                    viewModel.DetalleSolicitud.UsuarioId,
                    viewModel.DetalleSolicitud.Usuario?.NombreCompleto,
                    viewModel.DetalleSolicitud.EstadoCivil,
                    viewModel.DetalleSolicitud.PagaAlquiler,
                    viewModel.DetalleSolicitud.MontoAlquiler,
                    viewModel.DetalleSolicitud.NombreAcreedor,
                    viewModel.DetalleSolicitud.TotalCredito,
                    viewModel.DetalleSolicitud.AbonoSemanal,
                    viewModel.DetalleSolicitud.SaldoCredito,
                    viewModel.DetalleSolicitud.NombreDeudor,
                    viewModel.DetalleSolicitud.TotalPrestamo,
                    viewModel.DetalleSolicitud.SaldoPrestamo,
                    viewModel.DetalleSolicitud.TipoPrestamo,
                    viewModel.DetalleSolicitud.MontoSolicitud,
                    viewModel.DetalleSolicitud.PlazoMeses,
                    viewModel.DetalleSolicitud.CuotaSemanalSolicitud,
                    viewModel.DetalleSolicitud.PropositoPrestamo,
                    viewModel.DetalleSolicitud.EstadoSolicitud
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = "Solicitud no encontrada" }, JsonRequestBehavior.AllowGet);
        }

        //Actualiza las solicitudes de préstamo
        [HttpPost]
        public ActionResult ActualizarSolicitud(int id, string tipoPrestamo, decimal montoSolicitud, int plazoMeses, decimal cuotaSemanalSolicitud, string estadoSolicitud)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    // Buscar la solicitud por su ID
                    var solicitud = context.SolicitudesPrestamo.FirstOrDefault(s => s.solicitudPrestamoId == id);
                    if (solicitud == null)
                    {
                        return Json(new { success = false, error = "Solicitud no encontrada." });
                    }

                    // Actualizar las propiedades con los nuevos valores
                    solicitud.tipoPrestamo = tipoPrestamo;
                    solicitud.montoSolicitud = montoSolicitud;
                    solicitud.plazoMeses = plazoMeses;
                    solicitud.cuotaSemanalSolicitud = cuotaSemanalSolicitud;
                    solicitud.estadoSolicitud = estadoSolicitud;

                    // Guardar los cambios en la base de datos
                    context.Entry(solicitud).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    if (estadoSolicitud.ToLower() == "aprobada")
                    {
                        // Se pasa la solicitud y los parámetros necesarios para crear el préstamo.
                        bool respuesta = prestamoM.RegistrarPrestamoAprobado(solicitud, tipoPrestamo, montoSolicitud, plazoMeses, cuotaSemanalSolicitud);

                        if (!respuesta)  // Si no se registró correctamente, retornamos el error
                        {
                            return Json(new { success = false, error = "Error al registrar el préstamo aprobado." });
                        }
                        else
                        {
                            //Pendiente de revisar
                            return Json(new { success = "El prestamo fue aprobado" });
                        }
                    }


                    }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Manejar errores (podrías registrar el error, etc.)
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult ObtenerHistorialTransacciones(int prestamoId)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    // Primero obtenemos los datos crudos
                    var transaccionesBD = context.PrestamosTransacciones
                        .Where(pt => pt.prestamoId == prestamoId)
                        .OrderByDescending(pt => pt.fechaPago)
                        .ToList(); // ← Aquí ya ejecuta el SQL y pasa a memoria

                    // Ahora sí podemos formatear la fecha
                    var transacciones = transaccionesBD.Select(pt => new
                    {
                        pt.transaccionPrestamoId,
                        pt.montoAbonado,
                        fechaPago = pt.fechaPago.HasValue
                            ? pt.fechaPago.Value.ToString("dd/MM/yyyy")
                            : null
                    }).ToList();

                    if (!transacciones.Any())
                    {
                        return Json(new
                        {
                            success = false,
                            message = "No se encontraron transacciones para este préstamo."
                        }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new
                    {
                        success = true,
                        transacciones = transacciones
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = "Error al obtener el historial: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region   Vistas USUARIO

        public ActionResult ObtenerPrestamosAdmin()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var prestamos = context.Prestamos
                                       .Join(context.Usuario,
                                             prestamo => prestamo.usuarioId,
                                             usuario => usuario.usuarioId,
                                             (prestamo, usuario) => new
                                             {
                                                 PrestamoId = prestamo.prestamoId,
                                                 NombreAsociado = usuario.nombreCompleto,
                                                 TipoPrestamo = prestamo.tipoPrestamo,
                                                 MontoAprobado = prestamo.montoAprobado,
                                                 EstadoPrestamo = prestamo.estadoPrestamo
                                             })
                                       .ToList();

                return Json(prestamos, JsonRequestBehavior.AllowGet);
            }
        }



        //----------VISTAS ASOCIADO-----------//

        [Authorize]
        [HttpGet]
        public ActionResult SolicitudPrestamo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SolicitudPrestamo(Entities.SolicitudesPrestamo solicitud)
        {
            // Verificamos si existe el usuario en sesión
            if (Session["usuarioId"] == null)
            {
                // Opcional: redirige al login o muestra un mensaje
                return RedirectToAction("Login", "Account");
            }

            int usuarioId = Convert.ToInt32(Session["usuarioId"]);
            solicitud.UsuarioId = usuarioId;

            bool respuesta = prestamoM.RegistrarSolicitudPrestamo(solicitud);

            if (respuesta)
            {
                prestamoM.NotificacionSolicitudPrestamo(solicitud);
                TempData["SuccessMessage"] = "Solicitud enviada con éxito!";
                return RedirectToAction("SolicitudPrestamo"); // Redirige después del POST
            }
            else
            {
                ModelState.AddModelError("", "Ocurrió un error al registrar la solicitud.");
                return View();
            }
        }


        public ActionResult ObtenerPrestamosAsociado()
        {
            // Si la sesión es nula, usa el usuarioId 1 por problemas de conexion
            int usuarioId = Session["usuarioId"] != null ? (int)Session["usuarioId"] : 1;

            using (var context = new ASECCC_DIGITALEntities())
            {
                var prestamos = context.Prestamos
                                       .Where(p => p.usuarioId == usuarioId)
                                       .ToList();

                var prestamosViewModel = prestamos.Select(p => new
                {
                    PrestamoId = p.prestamoId,
                    TipoPrestamo = p.tipoPrestamo,
                    MontoAprobado = p.montoAprobado,
                    FechaSolicitud = p.fechaSolicitud.HasValue ? p.fechaSolicitud.Value.ToString("yyyy-MM-dd") : "",
                    EstadoPrestamo = p.estadoPrestamo,
                    SaldoPendiente = p.saldoPendiente,
                    CuotaSemanal = p.cuotaSemanal,
                    Plazo = p.plazo + " meses"
                }).ToList();

                return Json(prestamosViewModel, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ConsultaPrestamoAsociado()
        //  Lógica específica para la vista ConsultaPrestamoAsociado
        {
            return View();
        }



        }
    #endregion

}
