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
                        montoPendiente = cuenta.MontoPendiente,
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


        // Elimina una cuenta por cobrar
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

    }
}



