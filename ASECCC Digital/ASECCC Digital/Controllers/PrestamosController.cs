    using ASECCC_Digital.Models;
    using ASECCC_Digital.ViewModels;
    using ASECCC_Digital.Entities;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using ASECCC_Digital.Database;
using System.Linq;

    namespace ASECCC_Digital.Controllers
    {
        public class PrestamosController : Controller
        {
            // Instancia del modelo para la logica de negocio
            PrestamosModel prestamoM = new PrestamosModel();
           


        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);
                ViewBag.CurrentModule = "Prestamos"; // Asigno el CurrentModule para validarlo en el _MenuModulos
            }

        #region Vistas ADMIN
        //--------VISTAS ADMIN--------------//

        // GET: Prestamos
        public ActionResult Prestamo()
            {
                // Llamar a un método de prestamoM para obtener los datos necesarios
                //var prestamos = prestamoM.ObtenerListaPrestamosAdmin();
                return View();  // Pasar el modelo a la vista
            }

            public ActionResult RegistrarAbonos()
            {
                // Lógica específica para la vista RegistrarAbonos
                //var abonos = prestamoM.ObtenerAbonosPendientes();
                return View();
            }

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



        #endregion

        #region   Vistas USUARIO

        //----------VISTAS ASOCIADO-----------//


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
                TempData["SuccessMessage"] = "Solicitud enviada con éxito!";
                return RedirectToAction("SolicitudPrestamo"); // Redirige después del POST
            }
            else
            {
                ModelState.AddModelError("", "Ocurrió un error al registrar la solicitud.");
                return View();
            }
        }





        public ActionResult ConsultaPrestamoAsociado()
            {
                // Obtener los detalles de la consulta del préstamo asociado desde el modelo
                //var consulta = prestamoM.ObtenerConsultaPrestamoAsociado();
                return View();
            }



        }
    #endregion

}
