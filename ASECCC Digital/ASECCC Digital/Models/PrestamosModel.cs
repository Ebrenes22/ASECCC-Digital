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
        #region Metodos para Vistas de Administrador


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


        // Método para buscar un préstamo por nombre de usuario en vista RegistrarAbonos
        public List<PrestamoDetalleViewModel> ObtenerPrestamosPorUsuario(string NombreCompleto, out List<string> sugerencias)
        {
            sugerencias = new List<string>(); // Inicializa la lista de sugerencias

            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                // 1️⃣ Buscar usuario por coincidencia exacta o parcial
                var usuario = context.Usuario
                    .FirstOrDefault(u => u.nombreCompleto.Equals(NombreCompleto)); // Exacto

                if (usuario == null)
                {
                    // 2️⃣ Si no se encuentra el usuario exacto, obtener sugerencias de nombres similares
                    sugerencias = context.Usuario
                        .Where(u => u.nombreCompleto.Contains(NombreCompleto))
                        .Select(u => u.nombreCompleto)
                        .Distinct()
                        .Take(10)
                        .ToList();

                    return new List<PrestamoDetalleViewModel>(); // Devuelve una lista vacía si no hay coincidencia exacta
                }

                // 3️⃣ Si el usuario existe, buscar sus préstamos activos
                return context.Prestamos
                    .Where(p => p.usuarioId == usuario.usuarioId && p.estadoPrestamo == "activo")
                    .Select(p => new PrestamoDetalleViewModel
                    {
                        PrestamoId = p.prestamoId,
                        MontoAprobado = p.montoAprobado,
                        Plazo = p.plazo,
                        CuotaSemanal = p.cuotaSemanal,
                        TipoPrestamo = p.tipoPrestamo,
                        EstadoPrestamo = p.estadoPrestamo,
                        FechaSolicitud = (DateTime)p.fechaSolicitud,
                        SaldoPendiente = p.saldoPendiente
                    })
                    .ToList();
            }
        }


        //Metodo para registrar los abonos a los prestamos 
        public bool RegistrarAbono(PrestamoTransaccionViewModel model)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    using (var transaction = context.Database.BeginTransaction()) // Manejo de transacción
                    {
                        var prestamo = context.Prestamos.Find(model.PrestamoId);
                        if (prestamo == null) return false;

                        // Crear la nueva transacción de abono
                        var nuevaTransaccion = new Database.PrestamosTransacciones
                        {
                            prestamoId = model.PrestamoId, // ✅ Asignación correcta según la entidad
                            montoAbonado = model.MontoAbonado,
                            fechaPago = DateTime.Now
                        };

                        context.PrestamosTransacciones.Add(nuevaTransaccion);

                        // Actualizar saldo pendiente del préstamo
                        prestamo.saldoPendiente -= model.MontoAbonado;

                        // Evitar valores negativos y cambiar el estado si el préstamo se paga completamente
                        if (prestamo.saldoPendiente <= 0)
                        {
                            prestamo.estadoPrestamo = "Cancelado";
                            prestamo.saldoPendiente = 0;
                        }

                        context.SaveChanges();
                        transaction.Commit(); // Confirmar cambios en la base de datos
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Registro del error para depuración
                Console.WriteLine($"Error al registrar el abono: {ex.Message}");
                return false;
            }
        }



        #endregion



        #region Metodos para Vista de Usuario

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

        public bool NotificacionSolicitudPrestamo(Entities.SolicitudesPrestamo solicitud)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    // Buscar el usuario por su ID
                    var usuario = context.Usuario.FirstOrDefault(u => u.usuarioId == solicitud.UsuarioId);

                    // Verificar si el usuario existe
                    string nombreUsuario = usuario != null ? usuario.nombreCompleto : "Usuario desconocido";

                    // Crear la notificación
                    var notificacion = new Database.Notificaciones
                    {
                        usuarioId = solicitud.UsuarioId, // Usuario que trae la solicitud
                        titulo = "Nueva Solicitud de Préstamo",
                        contenido = $"Se ha recibido una nueva solicitud de préstamo de {nombreUsuario} por un monto de ¢{solicitud.MontoSolicitud}",
                        tipo = "General",
                        fechaEnvio = DateTime.Now,
                        estado = "enviada"
                    };

                    context.Notificaciones.Add(notificacion);
                    int rowsAffected = context.SaveChanges();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la notificación: {ex.Message}");
                return false;
            }
        }


                public List<object> ObtenerPrestamosParaAdmin()
        {
            using (var context = new ASECCC_DIGITALEntities()) // Conexión a la base de datos
            {
                return context.Prestamos
                              .Join(context.Usuario,
                                    prestamo => prestamo.usuarioId,
                                    usuario => usuario.usuarioId,
                                    (prestamo, usuario) => new
                                    {
                                        PrestamoId = prestamo.prestamoId,
                                        NombreAsociado = usuario.nombreCompleto,
                                        TipoPrestamo = prestamo.tipoPrestamo,
                                        MontoAprobado = prestamo.montoAprobado,
                                        EstadoPrestamo = prestamo.estadoPrestamo
                                    })
                              .ToList<object>(); 
            }
        }

        public List<Prestamos> ObtenerPrestamosAsociado(int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities()) // Conexión a la base de datos
            {
                return context.Prestamos
                              .Where(p => p.usuarioId == usuarioId)
                              .ToList(); // Retorna préstamos del usuario específico
            }
        }



        #endregion





    }
}
