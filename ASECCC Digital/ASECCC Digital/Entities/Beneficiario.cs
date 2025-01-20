using System;

namespace ASECCC_Digital.Entities
{
    public class Beneficiario
    {
        public int BeneficiarioId { get; set; }  // Identificador único del beneficiario
        public int UsuarioId { get; set; }  // Identificador del usuario al que pertenece el beneficiario
        public string NombreCompleto { get; set; }  // Nombre completo del beneficiario
        public string Relacion { get; set; }  // Relación con el usuario
        public decimal PorcentajeBeneficio { get; set; }  // Porcentaje de beneficio asignado

        // Navegación a la entidad Usuario (relación con la tabla Usuarios)
        public Usuario Usuario { get; set; }
    }
}