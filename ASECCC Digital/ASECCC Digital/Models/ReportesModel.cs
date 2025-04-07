using ASECCC_Digital.Database;
using ASECCC_Digital.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Models
{
    public class ReportesModel
    {

        public List<SelectListItem> GetUsuariosAsociados()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Usuario
                    .Where(u => u.rol == "asociado")
                    .Select(u => new SelectListItem
                    {
                        Value = u.usuarioId.ToString(),
                        Text = u.nombreCompleto
                    })
                    .ToList();
            }
        }

        public EstadoCuentaViewModel ObtenerEstadoCuentaPorUsuarioId(int usuarioId)
        {
            var model = new EstadoCuentaViewModel();

            using (var context = new ASECCC_DIGITALEntities())
            {
                model.UsuarioIdSeleccionado = usuarioId;

                model.UsuarioSeleccionado = context.Usuario
                    .FirstOrDefault(u => u.usuarioId == usuarioId);

                model.Ahorros = context.Ahorros
                    .Include("CatalogoTipoAhorro")
                    .Where(a => a.usuarioId == usuarioId && a.estado == "activo")
                    .ToList();

                model.Aportes = context.Aportes
                    .Where(a => a.usuarioId == usuarioId)
                    .ToList();

                model.Prestamos = context.Prestamos
                    .Where(p => p.usuarioId == usuarioId)
                    .ToList();

                model.Beneficios = context.BeneficiosServiciosCuenta
                    .Include("BeneficiosServicios")
                    .Where(b => b.usuarioId == usuarioId)
                    .ToList();
            }

            return model;
        }
    }
}
