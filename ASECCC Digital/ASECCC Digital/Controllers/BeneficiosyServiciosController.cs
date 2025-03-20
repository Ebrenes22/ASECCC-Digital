using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using ASECCC_Digital.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System;

namespace ASECCC_Digital.Controllers
{
    public class BeneficiosyServiciosController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "BenefyServ";
        }

        private BeneficioServicioModel beneficioServicioM = new BeneficioServicioModel();

        //--------VISTAS ADMIN--------------//

        public ActionResult BeneficioyServicio()
        {
            return View();
        }

        #region CRUD Vista GestionarBenefyServ
        public ActionResult GestionarBenefyServ()
        {
            var lista = beneficioServicioM.ConsultarBeneficioServicio(); // Retorna List<BeneficioServicio>
            var viewModel = new BeneficioServicioViewModel
            {
                BeneficioServicio = new BeneficioServicio(),
                BeneficioServicios = lista
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(BeneficioServicioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Se registra usando la información contenida en viewModel.BeneficioServicio
                bool registrado = beneficioServicioM.RegistrarBeneficioServicio(viewModel.BeneficioServicio);
                if (registrado)
                {
                    TempData["SuccessMessage"] = "El beneficio ha sido registrado exitosamente.";
                    return RedirectToAction("GestionarBenefyServ");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo registrar el beneficio.");
                }
            }
            // En caso de error, se recarga la vista con la lista actualizada
            viewModel.BeneficioServicios = beneficioServicioM.ConsultarBeneficioServicio();
            return View("GestionarBenefyServ", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(BeneficioServicioViewModel viewModel)

        {
            if (ModelState.IsValid)
            {

                bool actualizado = beneficioServicioM.ActualizarBeneficioServicio(viewModel.BeneficioServicio);
                if (actualizado)
                {
                    TempData["SuccessMessage"] = "El beneficio ha sido actualizado exitosamente.";
                    return RedirectToAction("GestionarBenefyServ");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo actualizar el beneficio.");
                }
            }
            // Si ocurre algún error, se recarga la vista con la lista actualizada y los datos ingresados

            {
                viewModel.BeneficioServicios = beneficioServicioM.ConsultarBeneficioServicio(); // O ConsultaBeneficioServicio() según tu método
                return View("GestionarBenefyServ", viewModel);
            };

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int BeneficioId)
        {
            bool eliminado = beneficioServicioM.EliminarBeneficioServicio(BeneficioId);
            if (eliminado)
            {
                TempData["SuccessMessage"] = "El beneficio ha sido eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar el beneficio.";
            }
            return RedirectToAction("GestionarBenefyServ");
        }

        #endregion


        public ActionResult RegistrarCuentaxCobrar()
        {
            // Obtenemos la lista de cuentas, usuarios y beneficios
            var cuentas = beneficioServicioM.ConsultarBeneficioServicioCuentas();
            var usuarios = beneficioServicioM.ConsultarUsuarios();
            var beneficios = beneficioServicioM.ConsultarBeneficioServicios();

            // Creamos el ViewModel con un objeto vacío para la nueva cuenta
            var viewModel = new BeneficioServicioViewModel
            {
                BeneficioServicioCuenta = new BeneficioServicioCuenta(),
                BeneficioServicioCuentas = cuentas,
                Usuarios = usuarios,
                BeneficioServicios = beneficios
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarCuenta(BeneficioServicioViewModel viewModel)
        {
            viewModel.BeneficioServicioCuentas = beneficioServicioM.ConsultarBeneficioServicioCuentas();
            viewModel.Usuarios = beneficioServicioM.ConsultarUsuarios();
            viewModel.BeneficioServicios = beneficioServicioM.ConsultarBeneficioServicios();

            if (ModelState.IsValid)
            {
                bool registrado = beneficioServicioM.RegistrarBeneficioServicioCuenta(viewModel.BeneficioServicioCuenta);
                if (registrado)
                {
                    beneficioServicioM.NotificacionCuentaporCobrar(viewModel.BeneficioServicioCuenta);
                    TempData["SuccessMessage"] = "La cuenta ha sido registrada exitosamente.";
                    return RedirectToAction("RegistrarCuentaxCobrar");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo registrar la cuenta.");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Por favor, revise los datos ingresados.";
            }

            return View("RegistrarCuentaxCobrar", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCuenta(int CuentaBeneficiosServiciosId)
        {
            bool eliminado = beneficioServicioM.EliminarBeneficioServicioCuenta(CuentaBeneficiosServiciosId);
            if (eliminado)
            {
                TempData["SuccessMessage"] = "La cuenta ha sido eliminada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la cuenta.";
            }
            return RedirectToAction("RegistrarCuentaxCobrar");
        }




        public ActionResult RegistrarAbonoBenefyServ()
        {
            return View();
        }



        //--------VISTAS ASOCIADOS--------------//

        public ActionResult BenefyServDisponibles()
        {
            var lista = beneficioServicioM.ConsultarBeneficioServicio();
            var listaActiva = lista.Where(b => b.Estado.Equals("activo", StringComparison.OrdinalIgnoreCase)).ToList();

            var viewModel = new BeneficioServicioViewModel
            {
                BeneficioServicio = new BeneficioServicio(),
                BeneficioServicios = listaActiva
            };

            return View(viewModel);
        }

        [HttpGet]
        public JsonResult ObtenerBeneficios()
        {
            var lista = beneficioServicioM.ConsultarBeneficioServicio();
            var listaActiva = lista.Where(b => b.Estado.Equals("activo", StringComparison.OrdinalIgnoreCase)).ToList();

            return Json(listaActiva, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ConsultarCuentasPorCobrar()
        {
            int usuarioId = ObtenerUsuarioIdLogueado();
            Console.WriteLine("Usuario en sesión para cuentas: " + usuarioId);

            if (usuarioId == -1)
            {
                return Json(new { success = false, message = "Asociado no encontrado." }, JsonRequestBehavior.AllowGet);
            }

            var resultado = beneficioServicioM.ObtenerCuentasPorCobrarAsociado(usuarioId);
            Console.WriteLine("Cuentas encontradas: " + resultado.Count);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ConsultarHistorialCuotas(int cuentaId)
        {
            var historial = beneficioServicioM.ObtenerHistorialCuotas(cuentaId);

            if (historial == null || !historial.Any())
            {
                return Json(new { success = false, message = "No hay transacciones registradas." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, data = historial }, JsonRequestBehavior.AllowGet);
        }



        private int ObtenerUsuarioIdLogueado()
        {
            if (Session["UsuarioId"] == null)
            {
                return -1;
            }

            return Convert.ToInt32(Session["UsuarioId"]);
        }


        public ActionResult ConsultarBenefyServAsociado()
        {
            return View();
        }

    }
}