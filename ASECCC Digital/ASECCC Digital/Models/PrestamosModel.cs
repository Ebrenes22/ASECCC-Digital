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
            int rowsAffected;
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
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
                        context.SolicitudesPrestamo.Add(tabladb);
                        rowsAffected = context.SaveChanges();  
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

        public List<Entities.SolicitudesPrestamo> ObtenerSolicitudesPorEstado(string estado)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                // Filtramos las solicitudes y las convertimos a SolicitudPrestamoViewModel
                var solicitudesFiltradas = context.SolicitudesPrestamo
                                                  .Where(s => s.estadoSolicitud == estado)
                                                  .Select(s => new Entities.SolicitudesPrestamo
                                                  {
                                                      SolicitudPrestamoId = s.solicitudPrestamoId,
                                                      UsuarioId = s.usuarioId,
                                                      MontoSolicitud = s.montoSolicitud,
                                                      EstadoSolicitud = s.estadoSolicitud
                                                  })
                                                  .ToList();
                return solicitudesFiltradas;
            }
        }

        public Database.SolicitudesPrestamo ObtenerSolicitudPorId(int id)
        {
            // Contexto de base de datos para obtener la solicitud de préstamo
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                // Buscar la solicitud por su ID
                var solicitud = context.SolicitudesPrestamo
                                       .FirstOrDefault(s => s.solicitudPrestamoId == id);

                // Si la solicitud es encontrada, se retorna
                return solicitud;
            }
        }




    }
}