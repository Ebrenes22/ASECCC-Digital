using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class SolicitudesPrestamo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SolicitudPrestamoId { get; set; }  // Identificador único de la solicitud de préstamo

        [Required(ErrorMessage = "El identificador del usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }  // Identificador del usuario que solicita el préstamo

        // Sección Datos Personales

        [Required(ErrorMessage = "El estado civil es requerido")]
        [StringLength(20, ErrorMessage = "El estado civil no puede superar los 20 caracteres")]
        public string EstadoCivil { get; set; }  // Estado civil del solicitante

        [Required(ErrorMessage = "El campo 'Paga Alquiler' es requerido")]
        public bool PagaAlquiler { get; set; }  // Indica si el solicitante paga alquiler

        [Range(0.01, 1000000, ErrorMessage = "Monto de alquiler fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MontoAlquiler { get; set; }  // Monto del alquiler mensual (opcional)

        // Deudas

        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string NombreAcreedor { get; set; }  // Nombre del acreedor de la deuda (opcional)

        [Range(0.01, double.MaxValue, ErrorMessage = "El total del crédito debe ser mayor que 0.")]
        public decimal? TotalCredito { get; set; }  // Monto total del crédito (opcional)

        [Range(0.01, double.MaxValue, ErrorMessage = "El abono semanal debe ser mayor que 0.")]
        public decimal? AbonoSemanal { get; set; }  // Monto semanal del abono (opcional)

        [Range(0.01, double.MaxValue, ErrorMessage = "El saldo del crédito debe ser mayor que 0.")]
        public decimal? SaldoCredito { get; set; }  // Saldo actual del crédito (opcional)

        // Fiador

        [Display(Name = "Nombre del Deudor")]
        public string NombreDeudor { get; set; }  // Nombre del deudor fiador (opcional)

        [Range(0, double.MaxValue, ErrorMessage = "El total del préstamo debe ser un número positivo.")]
        [Display(Name = "Total del Préstamo")]
        public decimal? TotalPrestamo { get; set; }  // Total del préstamo (opcional)

        [Range(0, double.MaxValue, ErrorMessage = "El saldo del préstamo debe ser un número positivo.")]
        [Display(Name = "Saldo del Préstamo")]
        public decimal? SaldoPrestamo { get; set; }  // Saldo del préstamo (opcional)

        //  Detalles Préstamo

        [Required(ErrorMessage = "El tipo de préstamo es requerido")]
        [StringLength(50, ErrorMessage = "El tipo de préstamo no puede superar los 50 caracteres")]
        public string TipoPrestamo { get; set; }  // Tipo de préstamo solicitado (urgente, personal, etc.)

        [Required(ErrorMessage = "El monto solicitado es requerido")]
        [Range(0.01, 1000000, ErrorMessage = "Monto solicitado fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoSolicitud { get; set; }  // Monto solicitado para el préstamo

        [Required(ErrorMessage = "El plazo en semanas es requerido")]
        [Range(1, 120, ErrorMessage = "El plazo debe estar entre 1 y 120 meses")]
        public int PlazoMeses { get; set; }  // Plazo en meses para el pago del préstamo

        [Required(ErrorMessage = "La cuota semanal solicitada es requerida")]
        [Range(0.01, 1000000, ErrorMessage = "Cuota semanal fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CuotaSemanalSolicitud { get; set; }  // Cuota semanal solicitada para el préstamo

        [Required(ErrorMessage = "El propósito del préstamo es requerido")]
        [StringLength(100, ErrorMessage = "La descripción no puede superar los 100 caracteres")]
        public string PropositoPrestamo { get; set; }  // Propósito para el cual se solicita el préstamo

        [Required(ErrorMessage = "El estado de la solicitud es requerido")]
        [RegularExpression("^(pendiente|revision|aprobada|rechazada)$", ErrorMessage = "Estado de solicitud inválido")]
        public string EstadoSolicitud { get; set; }  // Estado de la solicitud (pendiente, revisión, aprobado, rechazado)

        [Required(ErrorMessage = "La fecha de solicitud es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;  // Fecha y hora en que se realizó la solicitud del préstamo

        // Propiedad de navegación para relacionar la solicitud con un usuario
        public virtual Usuario Usuario { get; set; }  // Relación con la entidad Usuario


    }

}
