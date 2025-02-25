using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASECCC_Digital.Models
{
    public class SeguridadAuditoriaModel
    {
        public void RegistrarAuditoria(int usuarioId, string accion, string ipUsuario)
        {
            using (var db = new Database.ASECCC_DIGITALEntities())
            {
                var auditoria = new SeguridadAuditoria
                {
                    usuarioId = usuarioId,
                    accion = accion,
                    fechaAccion = DateTime.Now,
                    direccionIp = ipUsuario ?? "IP No disponible"
                };

                db.SeguridadAuditoria.Add(auditoria);
                db.SaveChanges();
            }
        }

        public List<SeguridadAuditoria> ObtenerRegistrosActividad(DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var db = new Database.ASECCC_DIGITALEntities())
            {
                var query = db.SeguridadAuditoria
                    .Include("Usuario")
                    .Where(a => a.accion == "Inicio de sesión")
                    .AsQueryable();

                if (fechaInicio.HasValue)
                {
                    query = query.Where(a => a.fechaAccion >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    fechaFin = fechaFin.Value.AddDays(1).AddTicks(-1);
                    query = query.Where(a => a.fechaAccion <= fechaFin.Value);
                }

                return query
                    .OrderByDescending(a => a.fechaAccion)
                    .ToList();
            }
        }
    }
}
