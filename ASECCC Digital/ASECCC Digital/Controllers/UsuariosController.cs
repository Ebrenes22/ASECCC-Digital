using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using ASECCC_Digital.Services;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using ASECCC_Digital.Database;
using System.Linq;

namespace ASECCC_Digital.Controllers
{
    public class UsuariosController : Controller
    {

        SeguridadAuditoriaModel auditoriaM = new SeguridadAuditoriaModel();
        private readonly ASECCC_DIGITALEntities _context = new ASECCC_DIGITALEntities();
        private readonly EmailService _emailService = new EmailService();

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

            //VARIABLES DE SESION
            Session["usuarioId"] = userEntity.UsuarioId;
            Session["usuarioNombre"] = userEntity.NombreCompleto;
            Session["usuarioIdentificacion"] = userEntity.Identificacion;



            string rol = userEntity.Rol.ToLower();

            // 3. Crear el ticket de FormsAuthentication con userData = rol
            var authTicket = new FormsAuthenticationTicket(
                version: 1,
                name: userEntity.Identificacion,          
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30),
                isPersistent: false,
                rol  
            );

            // 4. Encriptar el ticket y meterlo en una cookie
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                
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


        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(string ResetTipoIdentificacion, string ResetCedula)
        {
            if (string.IsNullOrEmpty(ResetTipoIdentificacion) || string.IsNullOrEmpty(ResetCedula))
            {
                return Json(new { success = false, message = "Debe completar todos los campos." });
            }

            // Buscar usuario según el tipo y número de identificación
            var usuario = _context.Usuario.FirstOrDefault(u => u.tipoIdentificacion == ResetTipoIdentificacion && u.identificacion == ResetCedula);
            if (usuario == null)
            {
                return Json(new { success = false, message = "No se encontró un usuario con esa identificación." });
            }

            // Generar un token seguro
            var token = GenerarTokenSeguro();
            usuario.resetToken = token;
            usuario.resetTokenExpiry = DateTime.UtcNow.AddHours(1);
            _context.SaveChanges();

            // Enviar correo con el enlace de recuperación
            var resetLink = Url.Action("RestablecerContrasena", "Usuarios", new { token = token }, Request.Url.Scheme);
            string mensaje = $"<p>Para restablecer tu contraseña, haz clic en el siguiente enlace:</p><p><a href='{resetLink}'>Restablecer Contraseña</a></p>";

            _emailService.EnviarCorreo(usuario.correoElectronico, "Recuperación de Contraseña", mensaje);

            return Json(new { success = true, message = "Se ha enviado un correo con las instrucciones." });
        }

        private string GenerarTokenSeguro()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenBytes = new byte[32];
                rng.GetBytes(tokenBytes);
                return Convert.ToBase64String(tokenBytes);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RestablecerContrasena(string token)
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.resetToken == token && u.resetTokenExpiry > DateTime.UtcNow);
            if (usuario == null)
            {
                TempData["Error"] = "El enlace es inválido o ha expirado.";
                return RedirectToAction("Login", "Usuarios");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RestablecerContrasena(string token, string nuevaContrasena)
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.resetToken == token && u.resetTokenExpiry > DateTime.UtcNow);
            if (usuario == null)
            {
                TempData["Error"] = "El enlace es inválido o ha expirado.";
                return RedirectToAction("Login", "Usuarios");
            }

            // Hash de la nueva contraseña
            usuario.contrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);
            usuario.resetToken = null;
            usuario.resetTokenExpiry = null;
            _context.SaveChanges();

            TempData["Success"] = "Tu contraseña ha sido restablecida.";
            return RedirectToAction("Login", "Usuarios");
        }


    }
}