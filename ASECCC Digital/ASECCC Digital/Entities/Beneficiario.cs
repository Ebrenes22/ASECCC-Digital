using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class Beneficiario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeneficiarioId { get; set; } // Identificador único del beneficiario

        [Required(ErrorMessage = "El usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; } // Identificador del usuario al que pertenece el beneficiario

        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Nombre debe tener entre 2 y 255 caracteres")]
        public string NombreCompleto { get; set; } // Nombre completo del beneficiario

        [Required(ErrorMessage = "La relación es requerida")]
        [StringLength(100, ErrorMessage = "La relación no puede exceder 100 caracteres")]
        public string Relacion { get; set; }  // Relación con el usuario

        [Required(ErrorMessage = "El porcentaje de beneficio es requerido")]
        [Range(0.01, 100, ErrorMessage = "Porcentaje debe estar entre 0.01 y 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeBeneficio { get; set; } // Porcentaje de beneficio asignado

        // Navegación a la entidad Usuario (relación con la tabla Usuarios)
        public Usuario Usuario { get; set; }
    }
}