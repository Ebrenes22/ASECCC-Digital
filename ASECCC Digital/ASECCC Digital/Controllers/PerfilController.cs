using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{


    public class PerfilController : BaseController
    {
        private BeneficiariosModel beneficiariosM = new BeneficiariosModel();

        protected override string GetCurrentModule()
        {
            return "Asociados";
        }


        //--------VISTAS USUARIOS-------------//

        // GET: Prestamos
        public ActionResult PerfilAsociado()
        {
            return View();
        }

        public ActionResult BeneficiariosAsociado()
        {
            return View();
        }


        // Obtener beneficiarios del usuario en sesión
        [HttpGet]
        public JsonResult ObtenerBeneficiarios()
        {
            try
            {
                if (Session["usuarioId"] == null)
                {
                    return Json(new { success = false, message = "Usuario no autenticado." }, JsonRequestBehavior.AllowGet);
                }

                int usuarioId = (int)Session["usuarioId"];
                var beneficiarios = beneficiariosM.ObtenerBeneficiarios(usuarioId);

                if (beneficiarios == null || !beneficiarios.Any())
                {
                    return Json(new { success = false, message = "No hay beneficiarios registrados." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    success = true,
                    beneficiarios = beneficiarios.Select(b => new
                    {
                        beneficiarioId = b.BeneficiarioId,
                        nombreCompleto = b.NombreCompleto,
                        relacion = b.Relacion,
                        porcentajeBeneficio = b.PorcentajeBeneficio
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // Registrar un nuevo beneficiario (Validando que el total de porcentaje no exceda 100%)
        [HttpPost]
        public JsonResult RegistrarBeneficiario(Beneficiario beneficiario)
        {
            if (Session["usuarioId"] == null)
                return Json(new { success = false, message = "Usuario no autenticado." });

            var nuevoBeneficiario = new ASECCC_Digital.Database.Beneficiarios
            {
                usuarioId = (int)Session["usuarioId"],
                nombreCompleto = beneficiario.NombreCompleto,
                relacion = beneficiario.Relacion,
                porcentajeBeneficio = beneficiario.PorcentajeBeneficio
            };

            var resultado = beneficiariosM.RegistrarBeneficiario(nuevoBeneficiario);
            return Json(new { success = resultado, message = resultado ? "Beneficiario agregado correctamente." : "El porcentaje total no puede exceder 100%." });
        }

        // Modificar los datos de un beneficiario existente
        [HttpPost]
        public JsonResult ModificarBeneficiario(Beneficiario beneficiario)
        {
            var beneficiarioDb = new ASECCC_Digital.Database.Beneficiarios
            {
                beneficiarioId = beneficiario.BeneficiarioId,
                usuarioId = beneficiario.UsuarioId,
                nombreCompleto = beneficiario.NombreCompleto,
                relacion = beneficiario.Relacion,
                porcentajeBeneficio = beneficiario.PorcentajeBeneficio
            };

            var resultado = beneficiariosM.ModificarBeneficiario(beneficiarioDb);
            return Json(new { success = resultado, message = resultado ? "Beneficiario actualizado correctamente." : "No se pudo actualizar el beneficiario." });
        }

        // Eliminar un beneficiario y redistribuir el porcentaje automáticamente
        [HttpPost]
        public JsonResult EliminarBeneficiario(int beneficiarioId)
        {
            if (Session["usuarioId"] == null)
                return Json(new { success = false, message = "Usuario no autenticado." });

            int usuarioId = (int)Session["usuarioId"];
            var resultado = beneficiariosM.EliminarBeneficiario(beneficiarioId, usuarioId);

            return Json(new { success = resultado, message = resultado ? "Beneficiario eliminado y porcentaje redistribuido." : "Debe quedar al menos un beneficiario activo." });
        }

        // Permite al usuario editar manualmente los porcentajes de beneficio
        [HttpPost]
        public JsonResult EditarPorcentajes(List<Beneficiario> beneficiarios)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                int usuarioId = (int)Session["usuarioId"];
                decimal totalNuevo = beneficiarios.Sum(b => b.PorcentajeBeneficio);

                if (totalNuevo > 100)
                {
                    return Json(new { success = false, message = "El total de porcentajes no puede exceder el 100%." });
                }

                foreach (var b in beneficiarios)
                {
                    var beneficiarioDb = context.Beneficiarios.Find(b.BeneficiarioId);
                    if (beneficiarioDb != null)
                    {
                        beneficiarioDb.porcentajeBeneficio = b.PorcentajeBeneficio;
                    }
                }

                context.SaveChanges();
                return Json(new { success = true, message = "Porcentajes actualizados correctamente." });
            }
        }
    }
}
