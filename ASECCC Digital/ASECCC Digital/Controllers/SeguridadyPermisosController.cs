using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class SeguridadyPermisosController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "SeguridadyPermisos"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }


        //--------VISTAS ADMIN--------------//
        // GET: SeguridadyPermisos

        public ActionResult SeguridadyPermiso()
        {
            return View();
        }
        public ActionResult RolesyPermisos()
        {
            return View();
        }

        public ActionResult RegistroActividadAuditoria()
        {
            var actividades = ObtenerRegistroDeActividad();
            return View(actividades);
      
        }

        private List<Actividad> ObtenerRegistroDeActividad()
        {
            // Simulación de datos para la tabla de auditoría
            return new List<Actividad>
            {
                new Actividad
                {
                    Usuario = "Admin1",
                    Rol = "Administrador",
                    FechaAcceso = DateTime.Now.Date,
                    HoraAcceso = DateTime.Now.AddHours(-2)
                },
                new Actividad
                {
                    Usuario = "User2",
                    Rol = "Asociado",
                    FechaAcceso = DateTime.Now.Date,
                    HoraAcceso = DateTime.Now.AddHours(-1)
                },
                new Actividad
                {
                    Usuario = "Admin2",
                    Rol = "Administrador",
                    FechaAcceso = DateTime.Now.Date,
                    HoraAcceso = DateTime.Now.AddHours(-3)
                }
            };
        }




        //--------VISTAS USUARIO-------------//
    }
    public class Actividad
    {
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public DateTime FechaAcceso { get; set; }
        public DateTime HoraAcceso { get; set; }
    }
}