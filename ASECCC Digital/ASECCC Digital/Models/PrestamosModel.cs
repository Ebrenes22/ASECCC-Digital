using ASECCC_Digital.Entities;
using ASECCC_Digital.Database;
using System.Data.Entity;
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

        //Para registro de nuevas solicitudes
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
                        estadoSolicitud = "Pendiente",
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

        // Método para registrar el préstamo aprobado
        public bool RegistrarPrestamoAprobado(Database.SolicitudesPrestamo solicitud,
                                                 string tipoPrestamo,
                                                 decimal montoSolicitud,
                                                 int plazoMeses,
                                                 decimal cuotaSemanalSolicitud)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    // Convertir el plazo de meses a semanas (ajusta el factor si es necesario)
                    var nuevoPrestamo = new Database.Prestamos
                    {
                        usuarioId = solicitud.usuarioId,
                        montoAprobado = montoSolicitud,
                        plazo = plazoMeses,
                        cuotaSemanal = cuotaSemanalSolicitud,
                        tipoPrestamo = tipoPrestamo.ToLower(),  // Convertir a minúsculas para cumplir con validaciones
                        estadoPrestamo = "activo", // Se marca como activo al ser aprobado
                        fechaSolicitud = solicitud.fechaSolicitud, // O DateTime.Now, según la lógica de negocio
                        fechaEstado = DateTime.Now,
                        saldoPendiente = montoSolicitud,
                        observaciones = "Préstamo aprobado automáticamente al cambiar el estado."
                    };

                    context.Prestamos.Add(nuevoPrestamo);
                    // Guardar los cambios y retornar verdadero si se afectó al menos una fila.
                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception )
            {
                // Opcional: registrar el error para depuración.
                return false;
            }
        }



        public List<Entities.SolicitudesPrestamo> ObtenerSolicitudesPorEstado(string estado)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var solicitudesFiltradas = context.SolicitudesPrestamo
                    .Where(s => s.estadoSolicitud == estado)
                    .Select(s => new Entities.SolicitudesPrestamo
                    {
                        SolicitudPrestamoId = s.solicitudPrestamoId,
                        UsuarioId = s.usuarioId,
                        MontoSolicitud = s.montoSolicitud,
                        EstadoSolicitud = s.estadoSolicitud,
                        FechaSolicitud = DbFunctions.TruncateTime(s.fechaSolicitud).Value,
                        // Proyectamos la información del usuario
                        Usuario = new Entities.Usuario
                        {
                            UsuarioId = s.Usuario.usuarioId,
                            NombreCompleto = s.Usuario.nombreCompleto
                        }
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
                FechaSolicitud = (DateTime)dbSolicitud.fechaSolicitud,

                Usuario = (dbSolicitud.Usuario != null ? new Entities.Usuario
                {
                    // Asegúrate de que en tu entidad Entities.Usuario exista la propiedad NombreCompleto
                    NombreCompleto = dbSolicitud.Usuario.nombreCompleto,
                    UsuarioId = dbSolicitud.Usuario.usuarioId // Puedes mapear otras propiedades si las necesitas
                } : null)
            };
        }


        public SolicitudPrestamoViewModel ObtenerSolicitudPorId(int id)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var solicitud = context.SolicitudesPrestamo
                                       .Include("Usuario")
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