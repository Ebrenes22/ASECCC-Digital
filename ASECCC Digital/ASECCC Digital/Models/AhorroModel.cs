using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Models
{

    public class AhorroModel
    {
        #region Metodos Vistas Administrador

        public JsonResult ConsultarAhorrosAsociados(string nombreAsociado)
        {
            try
            {

                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                    if (usuario == null)
                        return new JsonResult { Data = new { success = false, message = "Asociado no encontrado." } };
                    var ahorros = context.Ahorros.Where(a => a.usuarioId == usuario.usuarioId).ToList();
                    return new JsonResult { Data = new { success = true, ahorros = ahorros } };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Metodos Vistas Asociado

        public JsonResult RegistrarAhorro(string nombreAsociado, string tipoAhorro, decimal monto, int plazo)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
                try
                {
                    var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                    if (usuario == null)
                        return new JsonResult { Data = new { success = false, message = "Asociado no encontrado." } };
                    var nuevoAhorro = new Database.Ahorros
                    {
                        usuarioId = usuario.usuarioId,
                        tipoAhorroId = context.CatalogoTipoAhorro.FirstOrDefault(t => t.tipoAhorro == tipoAhorro).tipoAhorroId,
                        montoInicial = monto,
                        fechaInicio = DateTime.Now
                    };
                    context.Ahorros.Add(nuevoAhorro);
                    context.SaveChanges();
                    return new JsonResult { Data = new { success = true, message = "Ahorro registrado correctamente." } };
                }
                catch (Exception ex)
                {
                    return new JsonResult { Data = new { success = false, message = "Error al registrar el ahorro: " + ex.Message } };
                }
        }

        public JsonResult ModificarAhorro(int ahorroId, decimal nuevoMonto)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
                try
                {
                    var ahorro = context.Ahorros.Find(ahorroId);
                    if (ahorro == null)
                        return new JsonResult { Data = new { success = false, message = "Ahorro no encontrado." } };
                    ahorro.montoActual = nuevoMonto;
                    context.SaveChanges();
                    return new JsonResult { Data = new { success = true, message = "Monto del ahorro actualizado exitosamente." } };
                }
                catch (Exception ex)
                {
                    return new JsonResult { Data = new { success = false, message = "Error al modificar el ahorro: " + ex.Message } };
                }
        }
        #endregion

        public JsonResult EliminarAhorro(int ahorroId)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    var ahorro = context.Ahorros.Find(ahorroId);
                    if (ahorro == null) return new JsonResult { Data = new { success = false, message = "Ahorro no encontrado." } };
                    context.Ahorros.Remove(ahorro);
                    context.SaveChanges();
                    return new JsonResult { Data = new { success = true, message = "Ahorro eliminado exitosamente." } };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, message = "Error al eliminar el ahorro: " + ex.Message } };
            }
        }
    }
}
