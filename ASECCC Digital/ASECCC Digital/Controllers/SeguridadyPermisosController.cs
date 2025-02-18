using ASECCC_Digital.Models;
using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult RegistroActividadAuditoria()
        {
            return View(); 
        }

        [HttpGet]
        public JsonResult ObtenerDatosActividadAuditoria(DateTime? fechaInicio, DateTime? fechaFin, int pagina = 1)
        {
            int registrosPorPagina = 10;

            using (var db = new ASECCC_Digital.Database.ASECCC_DIGITALEntities())
            {
                var query = db.SeguridadAuditoria
                    .Include("Usuario")
                    .Where(a => a.accion == "Inicio de sesión")
                    .AsQueryable();

                if (fechaInicio.HasValue)
                {
                    query = query.Where(a => a.fechaAccion >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    fechaFin = fechaFin.Value.AddDays(1).AddTicks(-1);
                    query = query.Where(a => a.fechaAccion <= fechaFin.Value);
                }

                int totalRegistros = query.Count();

                var registros = query
                    .OrderByDescending(a => a.fechaAccion)
                    .Skip((pagina - 1) * registrosPorPagina)
                    .Take(registrosPorPagina)
                    .ToList() // Execute the query first
                    .Select(a => new
                    {
                        a.auditoriaId,
                        Usuario = a.Usuario != null ? a.Usuario.identificacion + " - " + a.Usuario.nombreCompleto : "Desconocido",
                        Fecha = a.fechaAccion.HasValue ? a.fechaAccion.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty,
                        a.direccionIp
                    })
                    .ToList();

                return Json(new
                {
                    registros = registros,
                    totalPaginas = Math.Ceiling((double)totalRegistros / registrosPorPagina),
                    paginaActual = pagina
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    }
