using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using static ASECCC_Digital.Controllers.AsociadosController;

namespace ASECCC_Digital.Models
{
    public class UsuariosModel
    {
        public bool UsuarioExiste(string identificacion)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                // Verifica si hay algún usuario con la misma identificación
                return context.Usuario.Any(u => u.identificacion == identificacion);
            }
        }

        public bool RegistrarAsociado(Entities.Usuario usuario)
        {
            int rowsAffected;

            var hashedContrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    var tabladb = new Database.Usuario
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
                    context.Usuario.Add(tabladb);
                    rowsAffected = context.SaveChanges();
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                // Manejo de excepciones



                return false;
            }
        }

        public Entities.Usuario Login(string identificacion, string contrasena)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario
                   .FirstOrDefault(u => u.identificacion == identificacion);

                if (usuarioDb == null)
                {
                    //No se encontro usuario con esa identificacion
                    return null;
                }
                if (usuarioDb.estadoAfiliacion == "inactivo")
                {
                    //Usuario inactivo
                    return null;
                }
                // Verificar la contraseña con BCrypt   
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(contrasena, usuarioDb.contrasena);
                if (!isValidPassword)
                {
                    // Contraseña inválida
                    return null;
                }
                var user = new Entities.Usuario
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

                return user;

            }
        }

        // Método para buscar un usuario por nombre
        public Database.Usuario BuscarUsuarioPorNombre(string nombre)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario
                    .FirstOrDefault(u => u.nombreCompleto.Contains(nombre));

                if (usuarioDb != null)
                {
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
                return null; // Si no se encuentra el usuario
            }
        }

        public bool ActualizarAsociado(Entities.Usuario usuario, bool actualizarRol = false)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario.Find(usuario.UsuarioId);

                if (usuarioDb != null)
                {
                    if (actualizarRol)
                    {
                        // Solo actualizar el rol si actualizarRol es true
                        usuarioDb.rol = usuario.Rol;
                    }
                    else
                    {
                        // Actualizar solo la información personal
                        usuarioDb.correoElectronico = usuario.CorreoElectronico;
                        usuarioDb.telefono = usuario.Telefono;
                        usuarioDb.direccion = usuario.Direccion;
                        usuarioDb.nombreCompleto = usuario.NombreCompleto;
                        usuarioDb.fechaNacimiento = usuario.FechaNacimiento;
                        usuarioDb.tipoIdentificacion = usuario.TipoIdentificacion;
                        usuarioDb.identificacion = usuario.Identificacion;

                    }

                    context.SaveChanges();
                    return true;
                }
                return false; // Si no se encuentra el usuario
            }
        }


        public bool DesactivarAsociado(int usuarioId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuario = context.Usuario.Find(usuarioId);

                if (usuario != null)
                {
                    usuario.estadoAfiliacion = "inactivo";
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public (List<object> cuentas, int usuarioId) BuscarCuentasAsociado(string nombre)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario.FirstOrDefault(u => u.nombreCompleto == nombre);
                if (usuarioDb == null)
                    return (new List<object>(), 0);

                int usuarioId = usuarioDb.usuarioId;

                // Obtener todas las cuentas en memoria
                var ahorros = context.Ahorros
                    .Where(a => a.usuarioId == usuarioId)
                    .ToList()
                    .Select(a => new
                    {
                        id = a.ahorroId,
                        tipo = "Ahorro",
                        descripcion = $"Identificación del ahorro: {a.ahorroId}",
                        saldo = a.montoActual
                    }).ToList<object>();

                var aportes = context.Aportes
                    .Where(a => a.usuarioId == usuarioId)
                    .ToList()
                    .Select(a => new
                    {
                        id = a.aporteId,
                        tipo = "Aporte",
                        descripcion = $"Aporte {a.tipoAporte}",
                        saldo = a.monto
                    }).ToList<object>();

                var prestamos = context.Prestamos
                    .Where(p => p.usuarioId == usuarioId)
                    .ToList()
                    .Select(p => new
                    {
                        id = p.prestamoId,
                        tipo = "Préstamo",
                        descripcion = $"Préstamo {p.tipoPrestamo}",
                        saldo = p.saldoPendiente ?? 0
                    }).ToList<object>();

                // Combinar todas las cuentas en una sola lista
                var cuentas = ahorros.Concat(aportes).Concat(prestamos).ToList();

                return (cuentas, usuarioId);
            }
        }




        public bool LiquidarCuenta(List<LiquidacionRequest> cuentas)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                try
                {
                    foreach (var cuenta in cuentas)
                    {
                        if (cuenta.Tipo == "Ahorro")
                        {
                            var ahorro = context.Ahorros.FirstOrDefault(a => a.ahorroId == cuenta.CuentaId);
                            if (ahorro != null)
                            {
                                ahorro.montoActual = 0;
                            }
                        }
                        else if (cuenta.Tipo == "Aporte")
                        {
                            var aporte = context.Aportes.FirstOrDefault(a => a.aporteId == cuenta.CuentaId);
                            if (aporte != null)
                            {
                                aporte.monto = 0;
                            }
                        }
                        else if (cuenta.Tipo == "Préstamo")
                        {
                            var prestamo = context.Prestamos.FirstOrDefault(p => p.prestamoId == cuenta.CuentaId);
                            if (prestamo != null)
                            {
                                prestamo.saldoPendiente = 0;
                            }
                        }
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }

        }
        public class LiquidacionRequest
        {
            public int CuentaId { get; set; }
            public string Tipo { get; set; }
        }

        public Usuario ObtenerInformacionPersonal(int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Usuario
                    .Where(u => u.usuarioId == usuarioId)
                    .FirstOrDefault();
            }
        }

        public bool ActualizarInformacionPersonal(int usuarioId, string correo, string telefono, string direccion)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var usuario = context.Usuario.Find(usuarioId);

                if (usuario == null)
                    return false;

                usuario.correoElectronico = correo;
                usuario.telefono = telefono;
                usuario.direccion = direccion;

                context.SaveChanges();
                return true;
            }
        }
    }
}





