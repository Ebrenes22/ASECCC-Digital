using ASECCC_Digital.Models;
using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;

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
            // Llamar a un método de prestamoM para la revisión de préstamos
            //var prestamosRevision = prestamoM.ObtenerPrestamosRevision();
            return View();
        }

        //----------VISTAS ASOCIADO-----------//
        

         [HttpGet]
        public ActionResult SolicitudPrestamo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SolicitudPrestamo(SolicitudesPrestamo solicitud)
        {
            var respuesta = prestamoM.RegistrarSolicitudPrestamo(solicitud);

            if (respuesta)
            {
                TempData["SuccessMessage"] = "Solicitud enviada con éxito!";
                return RedirectToAction("SolicitudExito");
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
