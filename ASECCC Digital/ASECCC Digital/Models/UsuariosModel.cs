using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public (bool esCuentaLiquidada, List<object> cuentas, int usuarioId) BuscarCuentasAsociado(string nombre)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario.FirstOrDefault(u => u.nombreCompleto == nombre);

                if (usuarioDb == null)
                {
                    return (false, new List<object>(), 0);
                }

                int usuarioId = usuarioDb.usuarioId;

                // Obtener datos de la base de datos 
                var ahorros = context.Ahorros
                    .Where(a => a.usuarioId == usuarioId)
                    .Select(a => new { a.tipoAhorroId, a.montoActual })
                    .ToList(); 

                var aportes = context.Aportes
                    .Where(a => a.usuarioId == usuarioId)
                    .Select(a => new { a.tipoAporte, a.monto })
                    .ToList();

                var prestamos = context.Prestamos
                    .Where(p => p.usuarioId == usuarioId)
                    .Select(p => new { p.tipoPrestamo, p.saldoPendiente })
                    .ToList();

                // Transformación en memoria 
                var cuentas = new List<object>();

                cuentas.AddRange(ahorros.Select(a => new { tipo = "Ahorro", descripcion = $"Tipo de Ahorro: {a.tipoAhorroId}", saldo = a.montoActual }));
                cuentas.AddRange(aportes.Select(a => new { tipo = "Aporte", descripcion = $"Tipo de Aporte: {a.tipoAporte}", saldo = a.monto }));
                cuentas.AddRange(prestamos.Select(p => new { tipo = "Préstamo", descripcion = $"Tipo de Préstamo: {p.tipoPrestamo}", saldo = p.saldoPendiente ?? 0 }));

                // Verificar si todas las cuentas están en 0 y el usuario está desactivado
                bool esCuentaLiquidada = cuentas.All(c => ((dynamic)c).saldo == 0) && usuarioDb.estadoAfiliacion == "inactivo";

                return (esCuentaLiquidada, cuentas, usuarioId);
            }
        }

    }
}




