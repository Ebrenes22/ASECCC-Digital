using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace ASECCC_Digital.Controllers
{

    public class AsociadosController : Controller
    {
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


        public ActionResult RegistrarAsociado()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarAsociado(Usuario model)
        {
            if (ModelState.IsValid) {
                // 1. Forzar el rol a "asociado"
                model.Rol = "asociado";

                // 2. Forzar el estado de afiliación a "activo"
                model.EstadoAfiliacion = "activo";

                // 3. Asignar la fecha de registro al momento actual
                model.FechaRegistro = DateTime.Now;

                // 4. Hashear la contraseña antes de guardarla
                model.HashedContrasena = HashPassword(model.Contrasena);

                // Nota: Si deseas no guardar la contraseña en texto plano en BD,
                //       puedes limpiar el campo "Contrasena":
                // model.Contrasena = null;

                // 5. Agregar y guardar en base de datos
                //db.Usuarios.Add(model);
               // db.SaveChanges();

                // Redirigimos a algún listado o página de confirmación
                return RedirectToAction("Index", "Home");
            }

            // Si el modelo no es válido, regresamos a la vista con los mensajes de error
            return View(model);
        }

        // Método de ayuda para hashear la contraseña
        private string HashPassword(string password)
        {
            // WorkFactor (cost) en 12 es generalmente seguro. 
            // Puedes ajustarlo dependiendo de rendimiento y seguridad.
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }


        public ActionResult ActualizarAsociado()
        {
            return View();
        }

        public ActionResult BuscarAsociado()
        {
            return View();
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