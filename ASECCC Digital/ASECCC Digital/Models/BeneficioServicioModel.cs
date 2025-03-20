using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ASECCC_Digital.Models
{
    public class BeneficioServicioModel
    {

        #region Metodos CRUD Vista GestionarBenefyServ
        public List<BeneficioServicio> ConsultarBeneficioServicio()
        {

            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosServicios
                              .Select(b => new BeneficioServicio
                              {
                                  BeneficioId = b.beneficioId,
                                  Nombre = b.nombre,
                                  Descripcion = b.descripcion,
                                  Requisitos = b.requisitos,
                                  Categoria = b.categoria,
                                  Estado = b.estado,
                                  FechaRegistro = (DateTime)b.fechaRegistro
                              })
                              .ToList();
            }
        }


        public bool RegistrarBeneficioServicio(BeneficioServicio beneficio)
        {
            int rowsAffected;
            try
            {

                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    var tabladb = new Database.BeneficiosServicios
                    {
                        nombre = beneficio.Nombre,
                        descripcion = beneficio.Descripcion,
                        requisitos = beneficio.Requisitos,
                        categoria = beneficio.Categoria,
                        estado = beneficio.Estado,
                        fechaRegistro = DateTime.Now
                    };

                    context.BeneficiosServicios.Add(tabladb);
                    rowsAffected = context.SaveChanges();
                    return rowsAffected > 0;

                }
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool ActualizarBeneficioServicio(BeneficioServicio beneficio)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {

                var beneficioActual = context.BeneficiosServicios.Find(beneficio.BeneficioId);
                if (beneficioActual == null)
                {
                    return false;
                }
                beneficioActual.nombre = beneficio.Nombre;
                beneficioActual.descripcion = beneficio.Descripcion;
                beneficioActual.requisitos = beneficio.Requisitos;
                beneficioActual.categoria = beneficio.Categoria;
                beneficioActual.estado = beneficio.Estado;

                return context.SaveChanges() > 0;
            }
        }

        public bool EliminarBeneficioServicio(int beneficioId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                // Buscar el registro por su ID
                var beneficio = context.BeneficiosServicios.Find(beneficioId);
                if (beneficio == null)
                {
                    return false;
                }

                context.BeneficiosServicios.Remove(beneficio);
                return context.SaveChanges() > 0;
            }
        }

        #endregion

        #region Metodos CRUD Vista RegistrarAbonoCuentaBeneficio
        public List<BeneficioServicioCuenta> ConsultarCuentasPorUsuario(int usuarioId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosServiciosCuenta
                              .Include(c => c.Usuario)
                              .Include(c => c.BeneficiosServicios)
                              .Where(c => c.usuarioId == usuarioId)
                              .Select(b => new BeneficioServicioCuenta
                              {
                                  CuentaBeneficiosServiciosId = b.cuentaBeneficiosServiciosId,
                                  UsuarioId = b.usuarioId,
                                  BeneficioId = b.beneficioId,
                                  MontoTotal = b.montoTotal,
                                  MontoPendiente = b.montoPendiente,
                                  NumeroProforma = b.numeroProforma,
                                  Plazo = b.plazo,
                                  FechaCreacion = (DateTime)b.fechaCreacion,
                                  Estado = b.estado,
                                  Usuario = new Usuario
                                  {
                                      UsuarioId = b.Usuario.usuarioId,
                                      NombreCompleto = b.Usuario.nombreCompleto
                                  },
                                  BeneficioServicio = new BeneficioServicio
                                  {
                                      BeneficioId = b.BeneficiosServicios.beneficioId,
                                      Nombre = b.BeneficiosServicios.nombre
                                  }
                              })
                              .ToList();
            }
        }

        public bool RegistrarAbono(BeneficioTransaccion abono)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    // Mapear las propiedades del abono a la entidad de base de datos
                    var dbAbono = new Database.BeneficiosTransacciones
                    {
                        // TransaccionId es autogenerado, así que no lo asignamos.
                        cuentaBeneficiosServiciosId = abono.CuentaBeneficiosServiciosId,
                        monto = abono.Monto,
                        // Puedes usar la fecha actual o el valor que venga en el objeto
                        fechaTransaccion = abono.FechaTransaccion, // o DateTime.Now,
                        descripcion = abono.Descripcion
                    };

                    context.BeneficiosTransacciones.Add(dbAbono);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        // Actualizar el monto pendiente en la cuenta correspondiente
                        var cuenta = context.BeneficiosServiciosCuenta.Find(abono.CuentaBeneficiosServiciosId);
                        if (cuenta != null)
                        {
                            // Suponiendo que el abono reduce el monto pendiente:
                            cuenta.montoPendiente = (cuenta.montoPendiente- abono.Monto);
                            context.SaveChanges();
                        }
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Opcional: registrar el error para depuración
                Console.WriteLine("Error al registrar abono: " + ex.Message);
                return false;
            }
        }

        public List<BeneficioTransaccion> ObtenerHistorialPagos(int cuentaId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosTransacciones
                              .Where(t => t.cuentaBeneficiosServiciosId == cuentaId)
                              .OrderByDescending(t => t.fechaTransaccion)
                              .Select(x => new BeneficioTransaccion
                              {
                                  TransaccionId = x.transaccionId,  // Asegúrate de que el nombre coincida
                                  CuentaBeneficiosServiciosId = x.cuentaBeneficiosServiciosId,
                                  Monto = x.monto,
                                  FechaTransaccion = (DateTime)x.fechaTransaccion, // O x.fechaTransaccion.Value si es nullable
                                  Descripcion = x.descripcion
                              })
                              .ToList();
            }
        }



        #endregion


        #region Metodos CRUD Vista RegistrarCuentaxCobrar
        public List<BeneficioServicioCuenta> ConsultarBeneficioServicioCuentas()
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosServiciosCuenta
                              .Include(c => c.Usuario)
                              .Include(c => c.BeneficiosServicios)
                              .Select(b => new BeneficioServicioCuenta
                              {
                                  CuentaBeneficiosServiciosId = b.cuentaBeneficiosServiciosId,
                                  UsuarioId = b.usuarioId,
                                  BeneficioId = b.beneficioId,
                                  MontoTotal = b.montoTotal,
                                  MontoPendiente = b.montoPendiente,
                                  NumeroProforma = b.numeroProforma,
                                  Plazo = b.plazo,
                                  FechaCreacion = (DateTime)b.fechaCreacion,
                                  Estado = b.estado,

                                  Usuario = new Entities.Usuario
                                  {
                                      UsuarioId = b.Usuario.usuarioId,
                                      NombreCompleto = b.Usuario.nombreCompleto
                                  },

                                  BeneficioServicio = new Entities.BeneficioServicio
                                  {
                                      BeneficioId = b.BeneficiosServicios.beneficioId,
                                      Nombre = b.BeneficiosServicios.nombre
                                  }
                              })
                              .ToList();
            }

        }

        public List<Usuario> ConsultarUsuarios()
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.Usuario
                    .Select(u => new Usuario
                    {
                        UsuarioId = u.usuarioId,
                        NombreCompleto = u.nombreCompleto
                    })
                    .ToList();
            }
        }

        public List<BeneficioServicio> ConsultarBeneficioServicios()
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosServicios
                    .Select(b => new BeneficioServicio
                    {
                        BeneficioId = b.beneficioId,
                        Nombre = b.nombre

                    })
                    .ToList();
            }
        }

        public bool RegistrarBeneficioServicioCuenta(BeneficioServicioCuenta cuenta)
        {
            int rowsAffected;
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    var tabladb = new Database.BeneficiosServiciosCuenta
                    {
                        usuarioId = cuenta.UsuarioId,
                        beneficioId = cuenta.BeneficioId,
                        montoTotal = cuenta.MontoTotal,
                        montoPendiente = cuenta.MontoTotal,
                        numeroProforma = cuenta.NumeroProforma,
                        plazo = cuenta.Plazo,
                        fechaCreacion = DateTime.Now,
                        estado = cuenta.Estado
                    };
                    context.BeneficiosServiciosCuenta.Add(tabladb);
                    rowsAffected = context.SaveChanges();
                    return rowsAffected > 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }



        public bool EliminarBeneficioServicioCuenta(int cuentaId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                var cuenta = context.BeneficiosServiciosCuenta.Find(cuentaId);
                if (cuenta == null)
                    return false;

                context.BeneficiosServiciosCuenta.Remove(cuenta);
                return context.SaveChanges() > 0;
            }
        }


        #endregion

        public bool NotificacionCuentaporCobrar(Entities.BeneficioServicioCuenta cuenta)
        {
            try
            {
                using (var context = new Database.ASECCC_DIGITALEntities())
                {
                    // Buscar el usuario por su ID
                    var usuario = context.Usuario.FirstOrDefault(u => u.usuarioId == cuenta.UsuarioId);
                    string nombreUsuario = usuario != null ? usuario.nombreCompleto : "Usuario desconocido";

                    // Obtener el beneficio usando el beneficioId
                    var beneficio = context.BeneficiosServicios.FirstOrDefault(b => b.beneficioId == cuenta.BeneficioId);
                    string nombreBeneficio = beneficio != null ? beneficio.nombre : "Beneficio desconocido";

                    // Crear la notificación
                    var notificacion = new Database.Notificaciones
                    {
                        usuarioId = cuenta.UsuarioId,
                        titulo = "Nueva Cuenta por Cobrar",
                        contenido = $"Se ha recibido una nueva cuenta por cobrar de {nombreUsuario} para el beneficio {nombreBeneficio} por un monto de ¢{cuenta.MontoTotal}",
                        tipo = "Personalizada",
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

        public List<BeneficioServicioCuenta> ObtenerCuentasPorCobrarAsociado(int usuarioId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosServiciosCuenta
                              .Include(c => c.BeneficiosServicios)
                              .Where(c => c.usuarioId == usuarioId)
                              .Select(c => new BeneficioServicioCuenta
                              {
                                  CuentaBeneficiosServiciosId = c.cuentaBeneficiosServiciosId,
                                  UsuarioId = c.usuarioId,
                                  BeneficioId = c.beneficioId,
                                  MontoTotal = c.montoTotal,
                                  MontoPendiente = c.montoPendiente,
                                  Plazo = c.plazo,
                                  FechaCreacion = (DateTime)c.fechaCreacion,
                                  Estado = c.estado,
                                  BeneficioServicio = new BeneficioServicio
                                  {
                                      BeneficioId = c.BeneficiosServicios.beneficioId,
                                      Nombre = c.BeneficiosServicios.nombre
                                  }
                              })
                              .ToList();
            }
        }

        public List<BeneficioTransaccion> ObtenerHistorialCuotas(int cuentaId)
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.BeneficiosTransacciones
                              .Where(t => t.cuentaBeneficiosServiciosId == cuentaId)
                              .Select(t => new BeneficioTransaccion
                              {
                                  TransaccionId = t.transaccionId,
                                  CuentaBeneficiosServiciosId = t.cuentaBeneficiosServiciosId,
                                  Monto = t.monto,
                                  FechaTransaccion = t.fechaTransaccion ?? DateTime.MinValue,
                                  Descripcion = t.descripcion
                              })
                              .ToList();
            }
        }

    }
}