using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASECCC_Digital.ViewModels
{
    public class SolicitudPrestamoViewModel
    {
        // Listado de solicitudes agrupadas por estado
        public SolicitudPrestamoLista Solicitudes { get; set; }

        // Detalle de una solicitud individual
        public SolicitudesPrestamo DetalleSolicitud { get; set; }

        // Modelo para insertar un nuevo préstamo
        public Prestamo NuevoPrestamo { get; set; }

        public ASECCC_Digital.Database.SolicitudesPrestamo SolicitdaDetalleBd { get; set; }

        // Constructor para inicializar las listas
        public SolicitudPrestamoViewModel()
        {
            Solicitudes = new SolicitudPrestamoLista
            {
                Pendientes = new List<SolicitudesPrestamo>(),
                EnRevision = new List<SolicitudesPrestamo>(),
                Aprobadas = new List<SolicitudesPrestamo>(),
                Rechazadas = new List<SolicitudesPrestamo>()
            };

            DetalleSolicitud = new SolicitudesPrestamo();
            NuevoPrestamo = new Prestamo();
        }

        public SolicitudPrestamoViewModel(ASECCC_Digital.Database.SolicitudesPrestamo dbSolicitud)
        {
            DetalleSolicitud = new SolicitudesPrestamo
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
                FechaSolicitud = DateTime.Now
            };
        }
    }
}
