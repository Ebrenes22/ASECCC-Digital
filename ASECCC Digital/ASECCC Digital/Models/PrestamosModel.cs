using ASECCC_Digital.Entities;
using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace ASECCC_Digital.Models
{
    public class PrestamosModel
    {

        public bool RegistrarSolicitudPrestamo(Entities.SolicitudesPrestamo solicitud)
        {
            if (solicitud == null)
            {
                throw new ArgumentNullException(nameof(solicitud), "La solicitud no puede ser nula.");
            }

            try
            {
                var tabladb = new Database.SolicitudesPrestamo
                {
                    usuarioId = solicitud.UsuarioId,
                    estadoCivil = solicitud.EstadoCivil,
                    pagaAlquiler = solicitud.PagaAlquiler,
                    montoAlquiler = solicitud.MontoAlquiler,
                    nombreAcreedor = solicitud.NombreAcreedor,
                    totalCredito = solicitud.TotalCredito,
                    abonoSemanal = solicitud.AbonoSemanal,
                    saldoCredito = solicitud.SaldoCredito,
                    nombreDeudor = solicitud.NombreDeudor,
                    totalPrestamo = solicitud.TotalPrestamo,
                    saldoPrestamo = solicitud.SaldoPrestamo,
                    tipoPrestamo = solicitud.TipoPrestamo,
                    montoSolicitud = solicitud.MontoSolicitud,
                    plazoMeses = solicitud.PlazoMeses,
                    cuotaSemanalSolicitud = solicitud.CuotaSemanalSolicitud,
                    propositoPrestamo = solicitud.PropositoPrestamo,
                    estadoSolicitud = "pendiente",
                    fechaSolicitud = DateTime.Now
                };

                using (var context = new ASECCC_DIGITALEntities())
                {
                    context.SolicitudesPrestamo.Add(tabladb);
                    int rowsAffected = context.SaveChanges();  // Método síncrono
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                // Manejo de excepciones, puede ser logueado o re-throw
                //Logger.LogError(ex, "Error al registrar la solicitud de préstamo");
                return false;
            }
        }







    }
}