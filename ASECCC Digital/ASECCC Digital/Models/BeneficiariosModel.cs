using System;
using System.Collections.Generic;
using System.Linq;
using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;

namespace ASECCC_Digital.Models
{
    public class BeneficiariosModel
    {
        public List<Beneficiario> ObtenerBeneficiarios(int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Beneficiarios
                    .Where(b => b.usuarioId == usuarioId)
                    .Select(b => new Beneficiario
                    {
                        BeneficiarioId = b.beneficiarioId,
                        UsuarioId = (int)b.usuarioId,
                        NombreCompleto = b.nombreCompleto,
                        Relacion = b.relacion,
                        PorcentajeBeneficio = b.porcentajeBeneficio
                    }).ToList();
            }
        }

        public bool RegistrarBeneficiario(Beneficiarios nuevoBeneficiario)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var totalPorcentaje = context.Beneficiarios
                    .Where(b => b.usuarioId == nuevoBeneficiario.usuarioId)
                    .Select(b => (decimal?)b.porcentajeBeneficio)
                    .DefaultIfEmpty(0)
                    .Sum() ?? 0;

                if (totalPorcentaje + nuevoBeneficiario.porcentajeBeneficio > 100)
                    return false;

                var beneficiarioDb = new Beneficiarios
                {
                    usuarioId = nuevoBeneficiario.usuarioId,
                    nombreCompleto = nuevoBeneficiario.nombreCompleto,
                    relacion = nuevoBeneficiario.relacion,
                    porcentajeBeneficio = nuevoBeneficiario.porcentajeBeneficio
                };

                context.Beneficiarios.Add(beneficiarioDb);
                context.SaveChanges();
                return true;
            }
        }

        public bool ModificarBeneficiario(Beneficiarios beneficiario)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var beneficiarioDb = context.Beneficiarios.Find(beneficiario.beneficiarioId);
                if (beneficiarioDb == null) return false;

                beneficiarioDb.nombreCompleto = beneficiario.nombreCompleto;
                beneficiarioDb.relacion = beneficiario.relacion;
                beneficiarioDb.porcentajeBeneficio = beneficiario.porcentajeBeneficio;

                context.SaveChanges();
                return true;
            }
        }

        public bool EliminarBeneficiario(int beneficiarioId, int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var beneficiarioDb = context.Beneficiarios.Find(beneficiarioId);
                if (beneficiarioDb == null) return false;

                var beneficiariosRestantes = context.Beneficiarios
                    .Where(b => b.usuarioId == usuarioId && b.beneficiarioId != beneficiarioId)
                    .ToList();

                if (!beneficiariosRestantes.Any()) return false;

                decimal porcentajeAReasignar = beneficiarioDb.porcentajeBeneficio;
                context.Beneficiarios.Remove(beneficiarioDb);
                context.SaveChanges();

                foreach (var b in beneficiariosRestantes)
                {
                    b.porcentajeBeneficio += porcentajeAReasignar / beneficiariosRestantes.Count;
                }

                context.SaveChanges();
                return true;
            }
        }
    }
}
