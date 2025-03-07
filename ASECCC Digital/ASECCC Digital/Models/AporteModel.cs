using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ASECCC_Digital.Models
{
    public class AporteModel
    {

        #region Metodos Vistas Administrador

        public object RegistrarAporte(string nombreAsociado, string tipoAporte, decimal monto)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
                try
                {
                    var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                    if (usuario == null)
                        return new { success = false, message = "Asociado no encontrado." };
                    var nuevoAporte = new Database.Aportes
                    {
                        usuarioId = usuario.usuarioId,
                        tipoAporte = tipoAporte,
                        monto = monto,
                        fechaRegistro = DateTime.Now
                    };
                    context.Aportes.Add(nuevoAporte);
                    context.SaveChanges();
                    return new { success = true, message = "Aporte registrado correctamente." };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al registrar el aporte: " + ex.Message };
                }
        }

        public object ModificarAporte(int aporteId, decimal nuevoMonto)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
                try
                {
                    var aporte = context.Aportes.Find(aporteId);
                    if (aporte == null)
                        return new { success = false, message = "Aporte no encontrado." };
                    aporte.monto = nuevoMonto;
                    context.SaveChanges();
                    return new { success = true, message = "Monto del aporte actualizado exitosamente." };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al modificar el aporte: " + ex.Message };
                }
        }

        public object EliminarAporte(int aporteId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
                try
                {
                    var aporte = context.Aportes.Find(aporteId);
                    if (aporte == null)
                        return new { success = false, message = "Aporte no encontrado." };
                    context.Aportes.Remove(aporte);
                    context.SaveChanges();
                    return new { success = true, message = "Aporte eliminado exitosamente." };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al eliminar el aporte: " + ex.Message };
                }
        }

        #endregion

        #region Metodos Vista Asociado

        public object ObtenerAportesPorAsociado(string nombreAsociado)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
                try
                {
                    var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                    if (usuario == null)
                        return new { success = false, message = "Asociado no encontrado." };

                    var aportes = context.Aportes
                        .Where(a => a.usuarioId == usuario.usuarioId)
                        .Select(a => new
                        {
                            AporteId = a.aporteId,
                            TipoAporte = a.tipoAporte,
                            Monto = a.monto,
                            FechaRegistro = a.fechaRegistro.HasValue ? a.fechaRegistro.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                            Estado = "Activo"
                        })
                        .ToList();

                    return new { success = true, data = aportes };
                }
                catch (Exception ex)
                {
                    return new { success = false, message = "Error al consultar los aportes: " + ex.Message };
                }
        }


        #endregion



        
    }

}
