using ASECCC_Digital.Entities;
using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using ASECCC_Digital.ViewModels;


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
                        montoAlquiler = solicitud.MontoAlquiler ?? 0m,  
                        nombreAcreedor = solicitud.NombreAcreedor ?? "No Aplica",
                        totalCredito = solicitud.TotalCredito ?? 0m,
                        abonoSemanal = solicitud.AbonoSemanal ?? 0m,
                        saldoCredito = solicitud.SaldoCredito ?? 0m,
                        nombreDeudor = solicitud.NombreDeudor ?? "No Aplica",
                        totalPrestamo = solicitud.TotalPrestamo ?? 0m,
                        saldoPrestamo = solicitud.SaldoPrestamo ?? 0m,
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

        public Entities.SolicitudesPrestamo MapearSolicitud(Database.SolicitudesPrestamo dbSolicitud)
        {
            return new Entities.SolicitudesPrestamo
            {
                SolicitudPrestamoId = dbSolicitud.solicitudPrestamoId,
                UsuarioId = dbSolicitud.usuarioId,
                EstadoCivil = dbSolicitud.estadoCivil,
                PagaAlquiler = dbSolicitud.pagaAlquiler,
                MontoAlquiler = dbSolicitud.montoAlquiler,
                NombreAcreedor = dbSolicitud.nombreAcreedor,
                TotalCredito = dbSolicitud.totalCredito,
                AbonoSemanal = dbSolicitud.abonoSemanal,
                SaldoCredito = dbSolicitud.saldoCredito,
                NombreDeudor = dbSolicitud.nombreDeudor,
                TotalPrestamo = dbSolicitud.totalPrestamo,
                SaldoPrestamo = dbSolicitud.saldoPrestamo,
                TipoPrestamo = dbSolicitud.tipoPrestamo,
                MontoSolicitud = dbSolicitud.montoSolicitud,
                PlazoMeses = dbSolicitud.plazoMeses,
                CuotaSemanalSolicitud = dbSolicitud.cuotaSemanalSolicitud,
                PropositoPrestamo = dbSolicitud.propositoPrestamo,
                EstadoSolicitud = dbSolicitud.estadoSolicitud,
                FechaSolicitud = (DateTime)dbSolicitud.fechaSolicitud
            };
        }


        public SolicitudPrestamoViewModel ObtenerSolicitudPorId(int id)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var solicitud = context.SolicitudesPrestamo
                                       .FirstOrDefault(s => s.solicitudPrestamoId == id);

                if (solicitud != null)
                {
                    // Mapear los datos de Database.SolicitudesPrestamo a Entities.SolicitudesPrestamo
                    var entidadSolicitud = MapearSolicitud(solicitud);

                    // Mapear la solicitud a un ViewModel
                    var viewModel = new SolicitudPrestamoViewModel
                    {
                        DetalleSolicitud = entidadSolicitud
                    };

                    return viewModel;
                }

                return null; // Si no se encuentra la solicitud
            }
        }





    }
}