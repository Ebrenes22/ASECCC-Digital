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
    }
}
