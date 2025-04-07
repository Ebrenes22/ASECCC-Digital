using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AhorroId { get; set; } // Identificador único del ahorro

        [Required(ErrorMessage = "El usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; } // Identificador del usuario que realiza el ahorro

        [Required(ErrorMessage = "El tipo de ahorro es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de ahorro válido")]
        public int TipoAhorroId { get; set; } // Referencia al catálogo de tipos de ahorro

        [Required(ErrorMessage = "El monto inicial es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto inicial debe ser mayor a cero")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoInicial { get; set; } // Monto inicial del ahorro

        [Required(ErrorMessage = "El monto actual es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto actual no puede ser negativo")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoActual { get; set; }  // Monto actual acumulado

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; } // Fecha de inicio del ahorro

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; } // Fecha de inicio del ahorro

        [Range(1, 120, ErrorMessage = "El plazo debe estar entre 1 y 120")]
        public int? Plazo { get; set; } // Plazo del ahorro en semanas o meses

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(activo|cancelado)$", ErrorMessage = "Estado inválido")]
        [StringLength(50)]
        public string Estado { get; set; } // Estado del ahorro (activo o cancelado)

        // Navegación a la entidad Usuario
        public Usuario Usuario { get; set; }

        // Agregar la relación con el catálogo de tipos de ahorro
        [ForeignKey("TipoAhorroId")]
        public CatalogoTipoAhorro TipoAhorro { get; set; }
        // Relación con las transacciones de ahorro

        public virtual CatalogoTipoAhorro CatalogoTipoAhorro { get; set; }
        public IEnumerable<Ahorro> Ahorros{ get; set; }
        public ICollection<AhorroTransaccion> AhorroTransacciones { get; set; }
    }
}