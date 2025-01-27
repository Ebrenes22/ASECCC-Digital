using ASECCC_Digital.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASECCC_Digital.Controllers
{
    public class UsuariosController : Controller
    {

        UsuariosModel usuarioM = new UsuariosModel();


        //--------VISTAS ADMIN--------------//
        // GET: Usuario
        public ActionResult Usuario()
        {
            return View();
        }

        // GET: Usuario/Create (Cargar la vista de registro)
        public ActionResult Registrar()
        {
            return View(new UsuariosModel());
        }

        // POST: Usuario/Create (Procesar el registro)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(UsuariosModel model)
        {
            if (ModelState.IsValid)
            {
                model.Rol = "asociado"; // Fijar el rol
                model.EstadoAfiliacion = "activo"; // Fijar el estado
                model.FechaRegistro = DateTime.Now; // Fecha actual
                model.HashedContrasena = HashPassword(model.Contrasena); // Hashear contraseña

                // Guardar el usuario en la base de datos
                // db.Usuarios.Add(model);
                // db.SaveChanges();

                return RedirectToAction("Asociados", "Asociados"); // Mensaje de confirmación
            }

            return View(model); // Devolver la vista si hay errores
        }

        // Método para hashear la contraseña
        private string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        }
        //Get: Usuario/Login
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Si el usuario ya está autenticado
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string tipoIdentificacion, string cedula, string password)
        {
            // 1. Validar datos mínimos
            if (string.IsNullOrWhiteSpace(tipoIdentificacion) ||
                string.IsNullOrWhiteSpace(cedula) ||
                string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Todos los campos son obligatorios.");
                return View();
            }

            // 2. Buscar el usuario en DB
            var usuario = db.Usuarios
                .FirstOrDefault(u => u.TipoIdentificacion == tipoIdentificacion &&
                                     u.Cedula == cedula);

            if (usuario == null)
            {
                ModelState.AddModelError("", "No existe un usuario con esa identificación.");
                return View();
            }

            // 3. Verificar la contraseña con BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, usuario.HashedContrasena);
            if (!isPasswordValid)
            {
                ModelState.AddModelError("", "Contraseña incorrecta.");
                return View();
            }

            // 4. Crear sesión o cookie de autenticación
            //    (Ejemplo: FormsAuthentication en MVC 5)
            FormsAuthentication.SetAuthCookie(usuario.Cedula, false);

            // Si quieres más control con roles, crea un FormsAuthenticationTicket con userData = rol
            var authTicket = new FormsAuthenticationTicket(
                1,                 // versión
                usuario.Cedula,    // nombre del usuario (User.Identity.Name)
                DateTime.Now,      // fecha de creación
                DateTime.Now.AddMinutes(120), // expiración
                false,             // persistCookie (remember me)
                usuario.Rol        // userData (rol del usuario)
            );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
                //configurar la expiración del cookie, usar SSL, etc.
            };
            Response.Cookies.Add(authCookie);

            // 5. Redirigir a la página que corresponda
            //    Podrías decidir si redirigir a un "Dashboard" distinto según Rol
            if (usuario.Rol == "admin")
            {
                //verificar "admin"
                return RedirectToAction("", "Home");
            }
            else
            {
                // rol "asociado"
                return RedirectToAction("", "Home");
            }
        }
        // GET: Usuarios/Logout
        [HttpGet]
        [Authorize] // Solo para usuarios logueados
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Usuarios");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
    //--------VISTAS USUARIO--------------//
}
