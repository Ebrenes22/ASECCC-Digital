using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace ASECCC_Digital.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }  // Identificador único del usuario

        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede superar los 100 caracteres")]
        public string NombreCompleto { get; set; }  // Nombre completo del usuario

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede superar los 100 caracteres")]
        public string CorreoElectronico { get; set; }  // Correo electrónico único

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contrasena { get; set; }  // Contraseña del usuario (antes de hashearla)

        public string HashedContrasena { get; set; }  // Propiedad para almacenar la contraseña hasheada

        [Required(ErrorMessage = "El tipo de identificación es requerido")]
        [RegularExpression("^(Nacional|DIMEX)$", ErrorMessage = "El tipo de identificación debe ser 'Nacional' o 'DIMEX'")]
        [StringLength(50)]
        public string TipoIdentificacion { get; set; }  // Tipo de identificación ('Nacional' o 'DIMEX')

        [Required(ErrorMessage = "El número de identificación es requerido")]
        [StringLength(20, ErrorMessage = "El número de identificación no puede superar los 20 caracteres")]
        public string Identificacion { get; set; }  // Número de identificación único

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }  // Fecha de nacimiento del usuario

        [Required(ErrorMessage = "El número de teléfono es requerido")]
        [Phone(ErrorMessage = "El número de teléfono no tiene un formato válido")]
        [StringLength(15, ErrorMessage = "El número de teléfono no puede superar los 15 caracteres")]
        public string Telefono { get; set; }  // Número de teléfono

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(250, ErrorMessage = "La dirección no puede superar los 250 caracteres")]
        public string Direccion { get; set; }  // Dirección completa

        [Required(ErrorMessage = "El rol es requerido")]
        [RegularExpression("^(administrador|asociado)$", ErrorMessage = "El rol debe ser 'administrador' o 'asociado'")]
        [StringLength(50)]
        public string Rol { get; set; }  // Rol del usuario ('administrador' o 'asociado')

        [Required(ErrorMessage = "El estado de afiliación es requerido")]
        [RegularExpression("^(activo|inactivo)$", ErrorMessage = "El estado de afiliación debe ser 'activo' o 'inactivo'")]
        [StringLength(50)]
        public string EstadoAfiliacion { get; set; }  // Estado de afiliación del usuario ('activo' o 'inactivo')

        [Required(ErrorMessage = "La fecha de registro es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }  // Fecha de registro del usuario
    }
}
