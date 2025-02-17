using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class AsociadosController : Controller
    {
        //Llama metodo registro desde UsuariosController
        UsuariosModel usuarioM = new UsuariosModel();

        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Asociados"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }
        //--------VISTAS ADMIN--------------//

        // GET: Asociados
        [Authorize]
        public ActionResult Asociados()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegistrarAsociado()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarAsociado(Usuario usuario)
        {

            // Verificar si el usuario ya existe en la base de datos
            if (usuarioM.UsuarioExiste(usuario.Identificacion))
            {
                TempData["Mensaje"] = "El usuario con esta identificación ya está registrado.";
                TempData["MensajeTipo"] = "error";
                return RedirectToAction("RegistrarAsociado");
            }

            var respuesta = usuarioM.RegistrarAsociado(usuario);

            if (respuesta)
            {
                TempData["Mensaje"] = "Usuario registrado correctamente.";
                TempData["MensajeTipo"] = "success";
                return RedirectToAction("RegistrarAsociado");
            }
            else
            {
                TempData["Mensaje"] = "Ocurrió un error al registrar el usuario.";
                TempData["MensajeTipo"] = "error";
                return RedirectToAction("RegistrarAsociado");
            }
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult BuscarAsociado(string buscarNombre)
        {
            // Buscar el usuario por nombre usando el modelo
            var usuario = usuarioM.BuscarUsuarioPorNombre(buscarNombre);

            if (usuario != null)
            {
                // Devolver los datos del usuario en formato JSON
                return Json(new
                {
                    success = true,
                    id = usuario.usuarioId,
                    nombre = usuario.nombreCompleto,
                    identificacion = usuario.identificacion,
                    fechaNacimiento = usuario.fechaNacimiento.ToString("yyyy-MM-dd"),
                    correo = usuario.correoElectronico,
                    telefono = usuario.telefono,
                    direccion = usuario.direccion,
                    tipo = usuario.tipoIdentificacion,
                    estado = usuario.estadoAfiliacion,
                    rol = usuario.rol
                });
            }
            else
            {
                // Si no se encuentra, devolver un mensaje de error
                return Json(new { success = false, message = "No se encontró ningún usuario con ese nombre." });
            }
        }

        [HttpGet]
        public ActionResult ActualizarAsociado()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarAsociado(Usuario usuario)
        {
            ModelState.Remove("Contrasena");
            ModelState.Remove("TipoIdentificacion");
            //ModelState.Remove("Rol");
            ModelState.Remove("EstadoAfiliacion");
            ModelState.Remove("FechaIngreso");

            if (ModelState.IsValid)
            {
                // Llamar al método del modelo para actualizar el usuario
                var resultado = usuarioM.ActualizarAsociado(usuario);

                if (resultado)
                {
                    TempData["Mensaje"] = "Usuario actualizado correctamente";
                    TempData["MensajeTipo"] = "success"; // Tipo de alerta para SweetAlert
                }
                else
                {
                    TempData["Mensaje"] = "No se pudo actualizar el usuario.";
                    TempData["MensajeTipo"] = "error"; // Tipo de alerta para SweetAlert
                }

                return RedirectToAction("ActualizarAsociado");
            }

            // Capturar errores de validación y enviarlos a la vista
            var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Errores"] = errores;
            TempData["MensajeTipo"] = "error"; // Tipo de alerta para SweetAlert

            return View(usuario);
        }

        [HttpGet]
        public ActionResult BuscarDesactivarAsociado()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult BuscarDesactivarAsociado(int usuarioId)
        {


            Console.WriteLine($"Solicitud para desactivar usuario con ID: {usuarioId}"); // Depuración en consola del servidor

            if (usuarioId <= 0)
            {
                return Json(new { success = false, message = "ID de usuario inválido." });
            }

            var resultado = usuarioM.DesactivarAsociado(usuarioId);

            if (resultado)
            {
                return Json(new { success = true, message = "Usuario desactivado correctamente." });
            }
            else
            {
                return Json(new { success = false, message = "No se pudo desactivar el usuario." });
            }
        }

        [HttpGet]
        [Authorize (Roles = "administrador")]
        public ActionResult LiquidarAsociado()
        {
            return View();
        }

    }





    //--------VISTAS USUARIO--------------//


}