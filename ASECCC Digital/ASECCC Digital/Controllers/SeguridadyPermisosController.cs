using ASECCC_Digital.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class SeguridadyPermisosController : Controller
    {
        UsuariosModel usuarioM = new UsuariosModel();
        SeguridadAuditoriaModel seguridadM = new SeguridadAuditoriaModel();


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

        [HttpGet]
        public ActionResult RegistroActividadAuditoria()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerDatosActividadAuditoria(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var registros = seguridadM.ObtenerRegistrosActividad(fechaInicio, fechaFin);

            var datos = registros.Select(a => new
            {
                a.auditoriaId,
                Usuario = a.Usuario != null ? a.Usuario.identificacion + " - " + a.Usuario.nombreCompleto : "Desconocido",
                Fecha = a.fechaAccion.HasValue ? a.fechaAccion.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty,
            });

            return Json(new { registros = datos }, JsonRequestBehavior.AllowGet);
        }
    }
}

