using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int rowsAffected = 0;

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
                    rowsAffected = context.SaveChanges();  // Método síncrono
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                // Manejo de excepciones, puede ser logueado o re-throw
                //Logger.LogError(ex, "Error al registrar el asociado");
                return false;
            }
        }

        public Entities.Usuario Login(string identificacion, string contrasena)
        {
            using (var context = new ASECCC_DIGITALEntities())
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
                //    Aquí suponemos que en "usuario.Contrasenna" guardamos el HASH bcrypt
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(contrasena, usuarioDb.contrasena);
                if (!isValidPassword)
                {
                    // Contraseña inválida
                    return null;
                }

                //  Si todo está bien, retornamos el usuario para usarlo en el Controller
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

        public bool ActualizarUsuario(Entities.Usuario usuario)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var usuarioDb = context.Usuario.Find(usuario.UsuarioId);

                if (usuarioDb != null)
                {
                    // Actualizar solo los campos editables
                    usuarioDb.correoElectronico = usuario.CorreoElectronico;
                    usuarioDb.telefono = usuario.Telefono;
                    usuarioDb.direccion = usuario.Direccion;
                    usuarioDb.estadoAfiliacion = usuario.EstadoAfiliacion;
                    usuarioDb.rol = usuario.Rol;

                    context.SaveChanges();
                    return true;
                }
                return false; // Si no se encuentra el usuario
            }
        }
    }
}




