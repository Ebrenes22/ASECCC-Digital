using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static ASECCC_Digital.Models.UsuariosModel;

namespace ASECCC_Digital.Controllers
{
    public class AsociadosController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "Asociados";
        }

        //Instancia de modelo de usuario
        UsuariosModel usuarioM = new UsuariosModel();

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
        public ActionResult ActualizarAsociado(Usuario usuario)
        {
            ModelState.Remove("Contrasena");
            ModelState.Remove("TipoIdentificacion");
            ModelState.Remove("Rol");
            ModelState.Remove("EstadoAfiliacion");
            ModelState.Remove("FechaIngreso");

            if (ModelState.IsValid)
            {
                // Llamar al método del modelo para actualizar el usuario
                var resultado = usuarioM.ActualizarAsociado(usuario);

                if (resultado)
                {
                    TempData["Mensaje"] = "Usuario actualizado correctamente";
                    TempData["MensajeTipo"] = "success"; 
                }
                else
                {
                    TempData["Mensaje"] = "No se pudo actualizar el usuario.";
                    TempData["MensajeTipo"] = "error"; 
                }

                return RedirectToAction("ActualizarAsociado");
            }

            // Capturar errores de validación y enviarlos a la vista
            var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Errores"] = errores;
            TempData["MensajeTipo"] = "error"; 

            return View(usuario);
        }

        [HttpGet]
        public ActionResult BuscarDesactivarAsociado()
        {
            return View();
        }

        [HttpPost]
        
        public JsonResult BuscarDesactivarAsociado(int usuarioId)
        {

            Console.WriteLine($"Solicitud para desactivar usuario con ID: {usuarioId}"); 

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
        [Authorize(Roles = "administrador")]
        public ActionResult LiquidarAsociado()
        {
            return View();
        }


        [HttpPost]
        public JsonResult BuscarCuentasAsociado(string buscarNombre)
        {
            try
            {
                UsuariosModel usuarioM = new UsuariosModel();
                var (cuentas, usuarioId) = usuarioM.BuscarCuentasAsociado(buscarNombre);

                if (usuarioId == 0)
                    return Json(new { success = false, message = "No se encontró el usuario." });

                return Json(new
                {
                    success = true,
                    usuarioId,
                    cuentas
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult LiquidarCuentas(List<LiquidacionRequest> cuentas)
        {
            try
            {
                
                bool resultado = usuarioM.LiquidarCuenta(cuentas);

                if (resultado)
                    return Json(new { success = true });
                else
                    return Json(new { success = false, message = "No se pudieron liquidar todas las cuentas." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado: " + ex.Message });
            }
        }



        //--------VISTAS ASOCIADO--------------//

        [HttpGet]
        public JsonResult ObtenerInformacionPersonal()
        {
            if (Session["usuarioId"] == null)
            {
                return Json(new { success = false, message = "No hay un usuario autenticado en la sesión." }, JsonRequestBehavior.AllowGet);
            }

            int usuarioId = (int)Session["usuarioId"];
            var usuario = usuarioM.ObtenerInformacionPersonal(usuarioId);

            if (usuario == null)
                return Json(new { success = false, message = "No se encontró la información del usuario." }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                success = true,
                usuario = new
                {
                    usuario.usuarioId,
                    usuario.nombreCompleto,
                    usuario.identificacion,
                    usuario.fechaNacimiento,
                    usuario.correoElectronico,
                    usuario.telefono,
                    usuario.direccion,
                    usuario.fechaIngreso
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActualizarInformacionPersonal(string correo, string telefono, string direccion)
        {
            if (Session["usuarioId"] == null)
            {
                return Json(new { success = false, message = "No hay un usuario autenticado en la sesión." });
            }

            int usuarioId = (int)Session["usuarioId"];
            bool actualizado = usuarioM.ActualizarInformacionPersonal(usuarioId, correo, telefono, direccion);

            if (actualizado)
            {
                return Json(new { success = true, message = "Información actualizada correctamente." });
            }
            else
            {
                return Json(new { success = false, message = "No se pudo actualizar la información." });
            }
        }
    }

}