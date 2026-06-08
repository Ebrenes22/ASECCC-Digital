using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // ← LÍNEA AGREGADA
using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;

namespace ASECCC_Digital.Models
{
    public class AhorroModel
    {
        #region Métodos para Asociado

        public List<Ahorros> ObtenerAhorrosPorAsociado(int usuarioId)
        {
            using (var db = new ASECCC_DIGITALEntities())
            {
                return db.Ahorros
                         .Include("CatalogoTipoAhorro")
                         .Where(a => a.usuarioId == usuarioId && a.estado != "eliminado")
                         .ToList();
            }
        }

         public bool SolicitarRetiro(int ahorroId,decimal montoRetiro,int usuarioSolicitanteId)
        {
            using (var db = new ASECCC_DIGITALEntities())
            {
                try
                {
                    var usuario = db.Usuario
                        .FirstOrDefault(x => x.usuarioId == usuarioSolicitanteId);

                    if (usuario == null)
                        return false;

                    var ahorro = db.Ahorros
                        .FirstOrDefault(x => x.ahorroId == ahorroId);

                    if (ahorro == null)
                        return false;

                    if (ahorro.tipoAhorroId != 1)
                        return false;

                    if (montoRetiro <= 0 || montoRetiro > ahorro.montoActual)
                        return false;

                    var administradores = db.Usuario
                        .Where(x => x.rol == "Administrador")
                        .ToList();

                    foreach (var admin in administradores)
                    {
                        db.Notificaciones.Add(new Notificaciones
                        {
                            usuarioId = admin.usuarioId,
                            titulo = "Solicitud de Retiro",
                            contenido =
                                $"El asociado {usuario.nombreCompleto} ha solicitado un retiro de ₡{montoRetiro:N2} de un ahorro A la Vista.",
                            tipo = "Personalizada",
                            fechaEnvio = DateTime.Now,
                            estado = "enviada"
                        });
                    }

                    db.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #region Métodos para Administrador

        public object ConsultarAhorrosAsociado(string nombreAsociado)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuario = context.Usuario.FirstOrDefault(u => u.nombreCompleto.Contains(nombreAsociado));
                if (usuario == null)
                    return new { success = false, message = "Asociado no encontrado." };

                var ahorros = context.Ahorros
                    .Include("CatalogoTipoAhorro") // ← AGREGADO PARA CONSISTENCIA
                    .Where(a => a.usuarioId == usuario.usuarioId && a.estado != "eliminado")
                    .Select(a => new
                    {
                        AhorroId = a.ahorroId,
                        TipoAhorro = a.CatalogoTipoAhorro.tipoAhorro,
                        MontoInicial = a.montoInicial,
                        MontoActual = a.montoActual,
                        FechaInicio = a.fechaInicio,
                        FechaFin = a.fechaFin,
                        Plazo = a.plazo,
                        Estado = a.estado
                    })
                    .ToList();

                return new { success = true, data = ahorros };
            }
        }

        public object RegistrarAhorroPorId(int usuarioId, string tipoAhorro, decimal monto, int plazo)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var tipoAhorroId = context.CatalogoTipoAhorro.FirstOrDefault(t => t.tipoAhorro == tipoAhorro)?.tipoAhorroId;
                if (tipoAhorroId == null)
                    return new { success = false, message = "Tipo de ahorro no válido." };

                var nuevoAhorro = new Ahorros
                {
                    usuarioId = usuarioId,
                    tipoAhorroId = tipoAhorroId.Value,
                    montoInicial = monto,
                    montoActual = 0,
                    fechaInicio = DateTime.Now,
                    fechaFin = DateTime.Now.AddMonths(plazo),
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

                ahorro.montoInicial = nuevoMonto;
                context.SaveChanges();
                return new { success = true, message = "Monto inicial actualizado exitosamente." };
            }
        }

        public object ObtenerHistorialAhorro(int ahorroId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var historial = context.AhorroTransacciones
                    .Where(t => t.ahorroId == ahorroId)
                    .Select(t => new
                    {
                        Fecha = t.fechaTransaccion,
                        Monto = t.monto,
                        Tipo = t.tipoTransaccionId == 1 ? "Depósito" : "Retiro",
                    })
                    .OrderByDescending(t => t.Fecha)
                    .ToList();

                return new { success = true, data = historial };
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
                        context.AhorroTransacciones.RemoveRange(transacciones);

                    context.Ahorros.Remove(ahorro);
                    context.SaveChanges();

                    return new { success = true, message = "Ahorro eliminado exitosamente." };
                }
                catch (Exception ex)
                {
                    return new
                    {
                        success = false,
                        message = "Error al eliminar el ahorro: " + (ex.InnerException?.Message ?? ex.Message)
                    };
                }
            }
        }

        public object FinalizarAhorro(int ahorroId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var ahorro = context.Ahorros.Include("CatalogoTipoAhorro").FirstOrDefault(a => a.ahorroId == ahorroId);
                if (ahorro == null)
                    return new { success = false, message = "Ahorro no encontrado." };

                if (ahorro.CatalogoTipoAhorro.tipoAhorro != "A la Vista")
                    return new { success = false, message = "Solo se pueden finalizar ahorros de tipo A la Vista." };

                return EliminarAhorro(ahorroId);
            }
        }

        public object AgregarAbonoAhorro(int ahorroId, decimal monto, string descripcion = null)
        {
            if (monto <= 0)
                return new { success = false, message = "Monto inválido." };

            using (var context = new ASECCC_DIGITALEntities())
            {
                try
                {
                    var ahorro = context.Ahorros.SingleOrDefault(a => a.ahorroId == ahorroId);
                    if (ahorro == null)
                        return new { success = false, message = "Ahorro no encontrado." };

                    if (ahorro.estado != "activo")
                        return new { success = false, message = "No se puede agregar abono a un ahorro inactivo o eliminado." };

                    ahorro.montoActual += monto;

                    var transaccion = new AhorroTransacciones
                    {
                        ahorroId = ahorroId,
                        tipoTransaccionId = 1, // Asegúrate que exista en catálogo
                        monto = monto,
                        fechaTransaccion = DateTime.Now,
                        descripcion = string.IsNullOrWhiteSpace(descripcion) ? "Abono agregado" : descripcion
                    };

                    context.AhorroTransacciones.Add(transaccion);
                    context.SaveChanges();

                    return new
                    {
                        success = true,
                        message = "Abono agregado correctamente.",
                        montoActual = ahorro.montoActual
                    };
                }
                catch (Exception ex)
                {
                    var baseEx = ex.GetBaseException();
                    return new
                    {
                        success = false,
                        message = "Error al agregar el abono: " + baseEx.Message
                    };
                }
            }
        }

        #endregion
    }
}