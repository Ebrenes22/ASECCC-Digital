using ASECCC_Digital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class SeguridadyPermisosController : Controller
    {
        UsuariosModel usuarioM = new UsuariosModel();
        

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
        [HttpGet]
        public ActionResult RolesyPermisos()
        {
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult RolesyPermisos(string nombre, string nuevoRol)
        {
            try
            {
                // Buscar el usuario en la base de datos por nombre
                var usuarioDb = usuarioM.BuscarUsuarioPorNombre(nombre);

                if (usuarioDb == null)
                {
                    return Json(new { success = false, message = "No se encontró el usuario." });
                }

                var usuario = new ASECCC_Digital.Entities.Usuario
                {
                    UsuarioId = usuarioDb.usuarioId,  
                    NombreCompleto = usuarioDb.nombreCompleto,
                    Rol = nuevoRol
                };
                
                var resultado = usuarioM.ActualizarAsociado(usuario, true);

                if (resultado)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Rol actualizado correctamente.",
                        id = usuario.UsuarioId,
                        nombre = usuarioDb.nombreCompleto,
                        identificacion = usuarioDb.identificacion,
                        nuevoRol = usuario.Rol
                    });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo actualizar el rol." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado: " + ex.Message });
            }
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