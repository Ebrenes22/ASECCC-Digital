using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                    return false; // No se encontró el registro a eliminar
                }
                // Eliminar el registro y guardar los cambios
                context.BeneficiosServicios.Remove(beneficio);
                return context.SaveChanges() > 0;
            }
        }
        #endregion





    }
}



