using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASECCC_Digital.Controllers
{
    public class UsuariosController : Controller
    {
        // Instancia del modelo para la lógica de negocio
        UsuariosModel usuarioM = new UsuariosModel();
        SeguridadAuditoriaModel auditoriaM = new SeguridadAuditoriaModel();
        //--------VISTAS ADMIN--------------//
        // GET: Usuario
        public ActionResult Usuario()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string identificacion, string contrasena)
        {
            // 1. Validaciones mínimas de nulos / vacíos
            if (string.IsNullOrWhiteSpace(identificacion) || string.IsNullOrWhiteSpace(contrasena))
            {
                ModelState.AddModelError("", "Debe ingresar tanto la identificación como la contraseña.");
                return View();
            }

            // 2. Llamar a la capa de modelo para verificar credenciales
            var usuarioModel = new UsuariosModel();
            var userEntity = usuarioModel.Login(identificacion, contrasena);

            if (userEntity == null)
            {

                TempData["LoginError"] = "Credenciales erroneas o Usuario Inactivo";
                return View();
            }

            // verificar si el usuario se encuentra activo
            if (userEntity.EstadoAfiliacion != "activo")
            {
                TempData["LoginError"] = "Usuario inactivo";
                return View();
            }

            string rol = userEntity.Rol.ToLower();

            // 3. Crear el ticket de FormsAuthentication con userData = rol
            var authTicket = new FormsAuthenticationTicket(
                version: 1,
                name: userEntity.Identificacion,          // User.Identity.Name
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30), // Ajusta el tiempo de sesión
                isPersistent: false,
                rol  // En userData guardamos el rol
            );

            // 4. Encriptar el ticket y meterlo en una cookie
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                // Puedes configurar más opciones, como tiempo de expiración, secure, etc.
            };

            // 5. Agregar la cookie al response
            Response.Cookies.Add(authCookie);

            auditoriaM.RegistrarAuditoria(userEntity.UsuarioId, "Inicio de sesión", Request.UserHostAddress); // Registrar actividad de auditoría

            // 6. Redirigir 
            // IMPORTANTE CON AUTH
            if (userEntity.Rol == "administrador")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (userEntity.Rol == "asociado")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["LoginError"] = "Rol no reconocido. Contacte al administrador.";
                return View();
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Elimina todas las variables de sesión
            return RedirectToAction("Login", "Usuarios");
        }

        

        public ActionResult ActualizarAsociado()
        {

            return View();
        }
        //--------VISTAS USUARIO--------------//
    }
}