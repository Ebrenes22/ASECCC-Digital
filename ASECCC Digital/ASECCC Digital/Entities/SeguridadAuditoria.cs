using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class SeguridadAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditoriaId { get; set; }  // Identificador único de la auditoría

        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; }  // Identificador del usuario que realizó la acción (puede ser null)

        [Required(ErrorMessage = "La acción realizada es requerida")]
        [StringLength(500, ErrorMessage = "La descripción de la acción no puede superar los 500 caracteres")]
        public string Accion { get; set; }  // Descripción de la acción realizada

        [Required(ErrorMessage = "La fecha de la acción es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaAccion { get; set; }  // Fecha de la acción realizada

        [Required(ErrorMessage = "La dirección IP es requerida")]
        [StringLength(50, ErrorMessage = "La dirección IP no puede superar los 50 caracteres")]
        public string DireccionIp { get; set; }  // Dirección IP del usuario que realizó la acción


        // Relación con la entidad Usuario (opcional, ya que puede ser null)
        public Usuario Usuario { get; set; }  // Relación con la entidad Usuario (si el usuario está presente)
    }
}
