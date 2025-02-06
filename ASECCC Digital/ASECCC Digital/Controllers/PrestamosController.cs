    using ASECCC_Digital.Models;
    using ASECCC_Digital.ViewModels;
    using ASECCC_Digital.Entities;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using ASECCC_Digital.Database;

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
            viewModel.Solicitudes.EnRevision = model.ObtenerSolicitudesPorEstado("En Revisión");
            viewModel.Solicitudes.Aprobadas = model.ObtenerSolicitudesPorEstado("Aprobado");
            viewModel.Solicitudes.Rechazadas = model.ObtenerSolicitudesPorEstado("Rechazado");

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
                    viewModel.DetalleSolicitud.UsuarioId,
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




            //----------VISTAS ASOCIADO-----------//


            [HttpGet]
            public ActionResult SolicitudPrestamo()
            {
                return View();
            }

            [HttpPost]
            public ActionResult SolicitudPrestamo(Entities.SolicitudesPrestamo solicitud)
            {
                var usuarioId = Session["usuarioId"] = 1;
                solicitud.UsuarioId = (int)usuarioId;
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
    }
