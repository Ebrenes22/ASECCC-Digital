using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
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

        private readonly ReportesModel reporteM = new ReportesModel();

        //--------VISTAS ADMIN--------------//
        public ActionResult Reporte()
        {
            return View();
        }

        public ActionResult GenerarEstadoAsociado()
        {
            int? usuarioId = Session["usuarioId"] as int?;
            var model = reporteM.ObtenerEstadoCuentaPorUsuarioId(usuarioId.Value);
            return View(model);
        }

        //--------VISTAS ASOCIADO--------------//

        public ActionResult EstadodeCuentaAsociados(int? UsuarioIdSeleccionado)
        {
            var model = new EstadoCuentaViewModel
            {
                Usuario = reporteM.GetUsuariosAsociados()
            };

            if (UsuarioIdSeleccionado.HasValue)
            {
                model = reporteM.ObtenerEstadoCuentaPorUsuarioId(UsuarioIdSeleccionado.Value);
                model.Usuario = reporteM.GetUsuariosAsociados();
            }

            return View("EstadodeCuentaAsociados", model);
        }


    }
}
