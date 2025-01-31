using ASECCC_Digital.Models;
using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASECCC_Digital.Controllers
{
    public class AsociadosController : Controller
    {
        //Llama metodo registro desde UsuariosController
        UsuariosController usuarioC = new UsuariosController();
        UsuariosModel usuarioM = new UsuariosModel();

        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Asociados"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }
        //--------VISTAS ADMIN--------------//

        // GET: Asociados
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
        public ActionResult RegistrarAsociado(Usuario usuario)
        {
            usuarioC.RegistrarAsociado(usuario);
            return View();
        }

        [HttpGet]
        public ActionResult ActualizarAsociado()
        {

            return View();
        }

        [HttpPost]
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

        [HttpPost]
        public ActionResult ActualizarAsociado(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Llamar al método del modelo para actualizar el usuario
                var resultado = usuarioM.ActualizarUsuario(usuario);

                if (resultado)
                {
                    TempData["Mensaje"] = "Usuario actualizado correctamente";
                    return RedirectToAction("ActualizarAsociado");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo actualizar el usuario.");
                }
            }

            // Si hay errores de validación, mostrar la vista nuevamente
            return View(usuario);
        }


        public ActionResult LiquidarAsociado()
        {
            return View();
        }

        public ActionResult BuscarDesactivarAsociado()
        {
            return View();
        }

        //--------VISTAS USUARIO--------------//


    }
}