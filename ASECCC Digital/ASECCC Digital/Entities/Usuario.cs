using System;

namespace ASECCC_Digital.Entities
{
    public class Usuario
    {
        public int UsuarioId { get; set; }  // Identificador único
        public string NombreCompleto { get; set; }  // Nombre completo del usuario
        public string CorreoElectronico { get; set; }  // Correo electrónico único
        public string Contrasena { get; set; }  // Contraseña del usuario
        public string TipoIdentificacion { get; set; }  // 'Nacional' o 'DIMEX'
        public string Identificacion { get; set; }  // Número de identificación único
        public DateTime FechaNacimiento { get; set; }  // Fecha de nacimiento del usuario
        public string Telefono { get; set; }  // Número de teléfono
        public string Direccion { get; set; }  // Dirección completa
        public string Rol { get; set; }  // 'administrador' o 'asociado'
        public string EstadoAfiliacion { get; set; }  // Estado de afiliación
        public DateTime FechaRegistro { get; set; }  // Fecha de registro
    }
}