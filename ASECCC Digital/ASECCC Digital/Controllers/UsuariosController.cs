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
        //--------VISTAS ADMIN--------------//
        // GET: Usuario
        public ActionResult Usuario()
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
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, se retorna la vista con los errores
                return View(usuario);
            }
            // Verificar si el usuario ya existe en la base de datos
            if (usuarioM.UsuarioExiste(usuario.Identificacion))
            {
                // Agregar un mensaje de error si el usuario ya existe
                ModelState.AddModelError("", "El usuario con esta identificación ya está registrado.");
                return View(usuario);
            }
            var respuesta = usuarioM.RegistrarAsociado(usuario);

            if (respuesta)
            {
                TempData["Mensaje"] = "Usuario registrado correctamente";
                return RedirectToAction("RegistrarAsociado, Asociados");
            }
            else
            {
                // Si hubo problema, muestra un mensaje de error
                ModelState.AddModelError("", "Ocurrió un error al registrar el usuario.");
                return View(usuario);
            }
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
                // No se encontró el usuario o contraseña inválida/inactivo
                ModelState.AddModelError("", "Identificación y/o contraseña inválidas, o usuario inactivo.");
                return View();
            }

            // 3. Crear el ticket de FormsAuthentication con userData = rol
            var authTicket = new FormsAuthenticationTicket(
                version: 1,
                name: userEntity.Identificacion,          // User.Identity.Name
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30), // Ajusta el tiempo de sesión
                isPersistent: false,
                userData: userEntity.Rol  // En userData guardamos el rol
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

            // 6. Redirigir según sea admin o asociado
            if (userEntity.Rol == "admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (userEntity.Rol == "asociado")
            {
                return RedirectToAction("Index", "Asociado");
            }
            else
            {
                // Rol desconocido => reenvía a Home
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult ActualizarAsociado()
        {

            return View();
        }
        //--------VISTAS USUARIO--------------//
    }
}