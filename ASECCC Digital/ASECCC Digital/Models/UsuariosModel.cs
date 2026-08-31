using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ASECCC_Digital.Models
{
    public class UsuariosModel
    {
        public bool UsuarioExiste(string identificacion)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Usuario
                    .AsNoTracking()
                    .Any(u => u.identificacion == identificacion);
            }
        }

        public bool RegistrarAsociado(Entities.Usuario usuario)
        {
            try
            {
                var hashedContrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

                using (var context = new ASECCC_DIGITALEntities())
                {
                    var usuarioDb = new Database.Usuario
                    {
                        nombreCompleto = usuario.NombreCompleto,
                        correoElectronico = usuario.CorreoElectronico,
                        contrasena = hashedContrasena,
                        tipoIdentificacion = usuario.TipoIdentificacion,
                        identificacion = usuario.Identificacion,
                        fechaNacimiento = usuario.FechaNacimiento,
                        telefono = usuario.Telefono,
                        direccion = usuario.Direccion,
                        rol = "asociado",
                        estadoAfiliacion = "activo",
                        fechaIngreso = DateTime.Now
                    };

                    context.Usuario.Add(usuarioDb);

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    $"Error al registrar asociado. Identificación: {usuario?.Identificacion}. Error: {ex}"
                );

                return false;
            }
        }

        public Entities.Usuario Login(string identificacion, string contrasena)
        {
            Database.Usuario usuarioDb;

            using (var context = new ASECCC_DIGITALEntities())
            {
                usuarioDb = context.Usuario
                    .AsNoTracking()
                    .FirstOrDefault(u => u.identificacion == identificacion);
            }

            if (usuarioDb == null)
            {
                return null;
            }

            if (string.Equals(
                usuarioDb.estadoAfiliacion,
                "inactivo",
                StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            bool isValidPassword;

            try
            {
                isValidPassword = BCrypt.Net.BCrypt.Verify(
                    contrasena,
                    usuarioDb.contrasena
                );
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    $"Error al verificar contraseña. Identificación: {identificacion}. Error: {ex}"
                );

                return null;
            }

            if (!isValidPassword)
            {
                return null;
            }

            return new Entities.Usuario
            {
                UsuarioId = usuarioDb.usuarioId,
                NombreCompleto = usuarioDb.nombreCompleto,
                Identificacion = usuarioDb.identificacion,
                FechaNacimiento = usuarioDb.fechaNacimiento,
                CorreoElectronico = usuarioDb.correoElectronico,
                Telefono = usuarioDb.telefono,
                Direccion = usuarioDb.direccion,
                TipoIdentificacion = usuarioDb.tipoIdentificacion,
                EstadoAfiliacion = usuarioDb.estadoAfiliacion,
                Rol = usuarioDb.rol
            };
        }

        public Database.Usuario BuscarUsuarioPorNombre(string nombre)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario
                    .AsNoTracking()
                    .FirstOrDefault(u => u.nombreCompleto.Contains(nombre));

                if (usuarioDb == null)
                {
                    return null;
                }

                return new Database.Usuario
                {
                    usuarioId = usuarioDb.usuarioId,
                    nombreCompleto = usuarioDb.nombreCompleto,
                    identificacion = usuarioDb.identificacion,
                    fechaNacimiento = usuarioDb.fechaNacimiento,
                    correoElectronico = usuarioDb.correoElectronico,
                    telefono = usuarioDb.telefono,
                    direccion = usuarioDb.direccion,
                    tipoIdentificacion = usuarioDb.tipoIdentificacion,
                    estadoAfiliacion = usuarioDb.estadoAfiliacion,
                    rol = usuarioDb.rol
                };
            }
        }

        public List<string> ObtenerSugerenciasNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<string>();
            }

            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Usuario
                    .AsNoTracking()
                    .Where(u => u.nombreCompleto.Contains(texto))
                    .Select(u => u.nombreCompleto)
                    .Distinct()
                    .Take(10)
                    .ToList();
            }
        }

        public bool ActualizarAsociado(
            Entities.Usuario usuario,
            bool actualizarRol = false)
        {
            try
            {
                using (var context = new ASECCC_DIGITALEntities())
                {
                    var usuarioDb = context.Usuario.Find(usuario.UsuarioId);

                    if (usuarioDb == null)
                    {
                        return false;
                    }

                    if (actualizarRol)
                    {
                        usuarioDb.rol = usuario.Rol;
                    }
                    else
                    {
                        usuarioDb.correoElectronico = usuario.CorreoElectronico;
                        usuarioDb.telefono = usuario.Telefono;
                        usuarioDb.direccion = usuario.Direccion;
                        usuarioDb.nombreCompleto = usuario.NombreCompleto;
                        usuarioDb.fechaNacimiento = usuario.FechaNacimiento;
                        usuarioDb.identificacion = usuario.Identificacion;
                    }

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    $"Error al actualizar asociado. UsuarioId: {usuario?.UsuarioId}. Error: {ex}"
                );

                return false;
            }
        }

        public bool DesactivarAsociado(int usuarioId)
        {
            try
            {
                using (var context = new ASECCC_DIGITALEntities())
                {
                    var usuario = context.Usuario.Find(usuarioId);

                    if (usuario == null)
                    {
                        return false;
                    }

                    usuario.estadoAfiliacion =
                        string.Equals(
                            usuario.estadoAfiliacion,
                            "activo",
                            StringComparison.OrdinalIgnoreCase)
                            ? "inactivo"
                            : "activo";

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    $"Error al cambiar estado del asociado. UsuarioId: {usuarioId}. Error: {ex}"
                );

                return false;
            }
        }

        public (List<object> cuentas, int usuarioId) BuscarCuentasAsociado(
            string nombre)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario
                    .AsNoTracking()
                    .FirstOrDefault(u => u.nombreCompleto == nombre);

                if (usuarioDb == null)
                {
                    return (new List<object>(), 0);
                }

                int usuarioId = usuarioDb.usuarioId;

                var ahorros = context.Ahorros
                    .AsNoTracking()
                    .Where(a => a.usuarioId == usuarioId)
                    .Select(a => new
                    {
                        id = a.ahorroId,
                        tipo = "Ahorro",
                        descripcion = "Estado del ahorro: " + a.estado,
                        saldo = a.montoActual
                    })
                    .ToList()
                    .Cast<object>()
                    .ToList();

                var aportes = context.Aportes
                    .AsNoTracking()
                    .Where(a => a.usuarioId == usuarioId)
                    .Select(a => new
                    {
                        id = a.aporteId,
                        tipo = "Aporte",
                        descripcion = "Aporte " + a.tipoAporte,
                        saldo = a.monto
                    })
                    .ToList()
                    .Cast<object>()
                    .ToList();

                var prestamos = context.Prestamos
                    .AsNoTracking()
                    .Where(p => p.usuarioId == usuarioId)
                    .Select(p => new
                    {
                        id = p.prestamoId,
                        tipo = "Préstamo",
                        descripcion = "Préstamo " + p.tipoPrestamo,
                        saldo = p.saldoPendiente ?? 0
                    })
                    .ToList()
                    .Cast<object>()
                    .ToList();

                var cuentas = ahorros
                    .Concat(aportes)
                    .Concat(prestamos)
                    .ToList();

                return (cuentas, usuarioId);
            }
        }

        public bool LiquidarCuenta(List<LiquidacionRequest> cuentas)
        {
            if (cuentas == null || !cuentas.Any())
            {
                return false;
            }

            try
            {
                using (var context = new ASECCC_DIGITALEntities())
                {
                    foreach (var cuenta in cuentas)
                    {
                        if (cuenta == null)
                        {
                            continue;
                        }

                        if (cuenta.Tipo == "Ahorro")
                        {
                            var ahorro = context.Ahorros
                                .FirstOrDefault(a => a.ahorroId == cuenta.CuentaId);

                            if (ahorro != null)
                            {
                                ahorro.montoActual = 0;
                            }
                        }
                        else if (cuenta.Tipo == "Aporte")
                        {
                            var aporte = context.Aportes
                                .FirstOrDefault(a => a.aporteId == cuenta.CuentaId);

                            if (aporte != null)
                            {
                                aporte.monto = 0;
                            }
                        }
                        else if (cuenta.Tipo == "Préstamo")
                        {
                            var prestamo = context.Prestamos
                                .FirstOrDefault(p => p.prestamoId == cuenta.CuentaId);

                            if (prestamo != null)
                            {
                                prestamo.saldoPendiente = 0;
                            }
                        }
                    }

                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    $"Error al liquidar cuentas. Error: {ex}"
                );

                return false;
            }
        }

        public class LiquidacionRequest
        {
            public int CuentaId { get; set; }
            public string Tipo { get; set; }
        }

        public Database.Usuario ObtenerInformacionPersonal(int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Usuario
                    .AsNoTracking()
                    .FirstOrDefault(u => u.usuarioId == usuarioId);
            }
        }

        public bool ActualizarInformacionPersonal(
            int usuarioId,
            string correo,
            string telefono,
            string direccion)
        {
            try
            {
                using (var context = new ASECCC_DIGITALEntities())
                {
                    var usuario = context.Usuario.Find(usuarioId);

                    if (usuario == null)
                    {
                        return false;
                    }

                    usuario.correoElectronico = correo;
                    usuario.telefono = telefono;
                    usuario.direccion = direccion;

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    $"Error al actualizar información personal. UsuarioId: {usuarioId}. Error: {ex}"
                );

                return false;
            }
        }
    }
}
