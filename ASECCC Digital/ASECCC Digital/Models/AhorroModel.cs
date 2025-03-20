using System;
using System.Linq;
using System.Web.Mvc;
using ASECCC_Digital.Database;

namespace ASECCC_Digital.Models
{
    public class AhorroModel
    {
        #region Métodos para Administrador

        public object ConsultarAhorrosAsociado(string nombreAsociado)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return new { success = false, message = "Asociado no encontrado." };

                var ahorros = context.Ahorros
                    .Where(a => a.usuarioId == usuario.usuarioId)
                    .Select(a => new
                    {
                        AhorroId = a.ahorroId,
                        TipoAhorro = a.CatalogoTipoAhorro.tipoAhorro,
                        MontoActual = a.montoActual,
                        FechaInicio = a.fechaInicio,
                        FechaFin = a.fechaFin,
                        Estado = a.estado
                    })
                    .ToList();

                return new { success = true, data = ahorros };
            }
        }

        public object RegistrarAhorro(string nombreAsociado, string tipoAhorro, decimal monto, int plazo)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return new { success = false, message = "Asociado no encontrado." };

                var tipoAhorroId = context.CatalogoTipoAhorro.FirstOrDefault(t => t.tipoAhorro == tipoAhorro)?.tipoAhorroId;
                if (tipoAhorroId == null)
                    return new { success = false, message = "Tipo de ahorro no válido." };

                var nuevoAhorro = new Ahorros
                {
                    usuarioId = usuario.usuarioId,
                    tipoAhorroId = tipoAhorroId.Value,
                    montoInicial = monto,
                    montoActual = monto,
                    fechaInicio = DateTime.Now,
                    plazo = plazo,
                    estado = "activo"
                };

                context.Ahorros.Add(nuevoAhorro);
                context.SaveChanges();
                return new { success = true, message = "Ahorro registrado correctamente." };
            }
        }

        public object ModificarAhorro(int ahorroId, decimal nuevoMonto)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var ahorro = context.Ahorros.Find(ahorroId);
                if (ahorro == null)
                    return new { success = false, message = "Ahorro no encontrado." };

                ahorro.montoActual = nuevoMonto;
                context.SaveChanges();
                return new { success = true, message = "Monto del ahorro actualizado exitosamente." };
            }
        }

        public object EliminarAhorro(int ahorroId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                try
                {
                    var ahorro = context.Ahorros.Find(ahorroId);
                    if (ahorro == null)
                        return new { success = false, message = "Ahorro no encontrado." };

                    var transacciones = context.AhorroTransacciones.Where(t => t.ahorroId == ahorroId).ToList();
                    if (transacciones.Any())
                    {
                        context.AhorroTransacciones.RemoveRange(transacciones);
                    }

                    context.Ahorros.Remove(ahorro);
                    context.SaveChanges();

                    return new { success = true, message = "Ahorro eliminado exitosamente." };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al eliminar el ahorro: " + ex.Message };
                }
            }
        }
        #endregion
    }
}
