using System;
using System.Collections.Generic;
using System.Linq;
using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;

namespace ASECCC_Digital.Models
{
    public class AporteModel
    {
        public object AportesPorAsociado(int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                try
                {
                    var usuario = context.Usuario.FirstOrDefault(u => u.usuarioId == usuarioId);
                    if (usuario == null)
                        return new { success = false, message = "Asociado no encontrado." };

                    var aportes = context.Aportes
                        .Where(a => a.usuarioId == usuarioId)
                        .OrderByDescending(a => a.fechaRegistro)
                        .ToList()
                        .Select(a => new ASECCC_Digital.Entities.Aporte
                        {
                            AporteId = a.aporteId,
                            UsuarioId = (int)a.usuarioId,
                            TipoAporte = a.tipoAporte,
                            Monto = a.monto,
                            FechaRegistro = (DateTime)a.fechaRegistro
                        }).ToList();

                    return new { success = true, data = aportes };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al consultar los aportes: " + ex.Message };
                }
            }
        }



        public object ObtenerHistorialAporte(int aporteId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                try
                {
                    var transacciones = context.AportesTransacciones
                        .Where(t => t.aporteId == aporteId)
                        .OrderByDescending(t => t.fechaTransaccion)
                        .Select(t => new
                        {
                            Fecha = t.fechaTransaccion,
                            Monto = t.monto,
                            Descripcion = t.descripcion
                        })
                        .ToList();

                    return new { success = true, data = transacciones };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al obtener el historial de transacciones: " + ex.Message };
                }
            }
        }


        public object RegistrarAporte(int usuarioId, string tipoAporte, decimal monto)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var nuevoAporte = new Aportes
                {
                    usuarioId = usuarioId,
                    tipoAporte = tipoAporte,
                    monto = monto,
                    fechaRegistro = DateTime.Now
                };

                context.Aportes.Add(nuevoAporte);
                context.SaveChanges();
                return new { success = true, message = "Aporte registrado correctamente." };
            }
        }

        public object ModificarAporte(int aporteId, decimal nuevoMonto)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var aporte = context.Aportes.Find(aporteId);
                if (aporte == null)
                    return new { success = false, message = "Aporte no encontrado." };

                aporte.monto = nuevoMonto;
                context.SaveChanges();
                return new { success = true, message = "Monto del aporte actualizado exitosamente." };
            }
        }

        public object EliminarAporte(int aporteId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                try
                {
                    var aporte = context.Aportes.Find(aporteId);
                    if (aporte == null)
                        return new { success = false, message = "Aporte no encontrado." };

                    var transacciones = context.AportesTransacciones.Where(t => t.aporteId == aporteId).ToList();
                    if (transacciones.Any())
                    {
                        context.AportesTransacciones.RemoveRange(transacciones);
                    }

                    context.Aportes.Remove(aporte);
                    context.SaveChanges();

                    return new { success = true, message = "Aporte eliminado exitosamente." };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al eliminar el aporte: " + ex.Message };
                }
            }
        }

        //Admin

        public object ConsultarAportesPorNombre(string nombreAsociado)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return new { success = false, message = "Asociado no encontrado." };

                var aportes = context.Aportes
                    .Where(a => a.usuarioId == usuario.usuarioId)
                    .OrderByDescending(a => a.fechaRegistro)
                    .Select(a => new
                    {
                        AporteId = a.aporteId,
                        TipoAporte = a.tipoAporte,
                        Monto = a.monto,
                        FechaRegistro = a.fechaRegistro
                    })
                    .ToList();

                return new { success = true, data = aportes };
            }
        }

    }
}
