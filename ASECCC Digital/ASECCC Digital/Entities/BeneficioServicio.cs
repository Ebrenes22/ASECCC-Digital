using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class BeneficioServicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeneficioId { get; set; }  // Identificador único del beneficio o servicio

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Nombre debe tener entre 2 y 255 caracteres")]
        public string Nombre { get; set; }  // Nombre del beneficio o servicio

        [StringLength(1000, ErrorMessage = "Descripción no puede exceder 1000 caracteres")]
        public string Descripcion { get; set; } // Descripción detallada del beneficio o servicio

        [StringLength(100, ErrorMessage = "Categoría no puede exceder 100 caracteres")]
        public string Categoria { get; set; } // Categoría del beneficio o servicio

        [StringLength(500, ErrorMessage = "Requisitos no pueden exceder 500 caracteres")]
        public string Requisitos { get; set; }  // Requisitos para acceder al beneficio o servicio

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(activo|inactivo)$", ErrorMessage = "Estado inválido")]
        [StringLength(50)]
        public string Estado { get; set; } // Estado del beneficio o servicio (activo o inactivo)

        [Required(ErrorMessage = "La fecha de registro es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now; // Fecha en la que se registra el beneficio o servicio
    }
}