using ASECCC_Digital.Database;
using ASECCC_Digital.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class ReportesController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "Reportes";
        }

        //--------VISTAS ADMIN--------------//
        public ActionResult Reporte()
        {
            return View();
        }

        public ActionResult EstadodeCuentaAsociados()
        {
            return RedirectToAction("GenerarEstadoAsociado", "Reportes");
        }


        public ActionResult GenerarEstadoAsociado(int? UsuarioIdSeleccionado)
        {
            var model = new EstadoCuentaViewModel();

            using (var context = new ASECCC_DIGITALEntities())
            {
                // Cargar usuarios para el dropdown
                model.Usuario = context.Usuario
                    .Select(u => new SelectListItem
                    {
                        Value = u.usuarioId.ToString(),
                        Text = u.nombreCompleto
                    }).ToList();

                if (UsuarioIdSeleccionado.HasValue)
                {
                    model.UsuarioIdSeleccionado = UsuarioIdSeleccionado.Value;

                    // Cargar el usuario completo
                    model.UsuarioSeleccionado = context.Usuario
                        .FirstOrDefault(u => u.usuarioId == UsuarioIdSeleccionado.Value);

                    model.Ahorros = context.Ahorros
                        .Where(a => a.usuarioId == UsuarioIdSeleccionado.Value)
                        .ToList();

                    model.Aportes = context.Aportes
                        .Where(a => a.usuarioId == UsuarioIdSeleccionado.Value)
                        .ToList();

                    model.Prestamos = context.Prestamos
                        .Where(p => p.usuarioId == UsuarioIdSeleccionado.Value)
                        .ToList();

                    model.Beneficios = context.BeneficiosServiciosCuenta
                        .Where(b => b.usuarioId == UsuarioIdSeleccionado.Value)
                        .ToList();
                }

            }


            return View("EstadodeCuentaAsociados", model);
        }

        //--------VISTAS USUARIO-------------//
        // Si vas a tener una vista diferente para usuarios, puedes dejar esta vacía o eliminarla si no se usa
        // public ActionResult GenerarEstadoAsociado()
        // {
        //     return View();
        // }
    }
}
