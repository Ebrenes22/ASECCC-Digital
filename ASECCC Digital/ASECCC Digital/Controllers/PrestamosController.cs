using ASECCC_Digital.Models;
using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class PrestamosController : Controller
    {
        // Instancia del modelo para la logica de negocio
        PrestamosModel prestamoM = new PrestamosModel();

        //Instancias de las entidades
        SolicitudesPrestamo solicitudE = new SolicitudesPrestamo();
        Prestamo prestamoE = new Prestamo();



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

        public ActionResult SolicitudPrestamo()
        {
            // Crear una nueva instancia del modelo y pasarla a la vista
            //var registrar = prestamoM.RegistrarSolicitud();  // Llamar a un método que puede cargar los datos necesarios
            return View(solicitudE);  // Pasar el modelo a la vista
        }

        public ActionResult ConsultaPrestamoAsociado()
        {
            // Obtener los detalles de la consulta del préstamo asociado desde el modelo
            //var consulta = prestamoM.ObtenerConsultaPrestamoAsociado();
            return View();
        }
    }
}
