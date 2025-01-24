using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class Notificacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificacionId { get; set; }  // Identificador único de la notificación.

        [Required(ErrorMessage = "El usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }  // Clave foránea que referencia al usuario que recibe la notificación.

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Título debe tener entre 2 y 255 caracteres")]
        public string Titulo { get; set; }  // Título de la notificación.

        [Required(ErrorMessage = "El contenido es requerido")]
        [StringLength(1000, ErrorMessage = "Contenido no puede exceder 1000 caracteres")]
        public string Contenido { get; set; }  // Contenido de la notificación.

        [Required(ErrorMessage = "El tipo de notificación es requerido")]
        [RegularExpression("^(general|personalizada)$", ErrorMessage = "Tipo de notificación inválido")]
        [StringLength(50)]
        public string Tipo { get; set; }  // Tipo de la notificación (general o personalizada).

        [Required(ErrorMessage = "La fecha de envío es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaEnvio { get; set; }  // Fecha y hora de envío de la notificación.

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(enviada|leida)$", ErrorMessage = "Estado inválido")]
        [StringLength(50)]
        public string Estado { get; set; }  // Estado de la notificación (enviada o leída).

        public Usuario Usuario { get; set; }  // Propiedad de navegación que establece la relación con la entidad Usuario.
    }
}
