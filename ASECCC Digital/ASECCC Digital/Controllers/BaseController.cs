using ASECCC_Digital.Models;
using ASECCC_Digital.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = GetCurrentModule();

            var session = System.Web.HttpContext.Current.Session;

            if (session["usuarioId"] != null)
            {
                int usuarioId = Convert.ToInt32(session["usuarioId"]);
                var modelo = new NotificacionModel();

                var generales = modelo.ObtenerNoLeidasGenerales();
                var personalizadas = modelo.ObtenerNoLeidasPorUsuario(usuarioId);

                System.Web.HttpContext.Current.Items["NotificacionesPendientes"] =
                    generales.Count + personalizadas.Count;

                System.Web.HttpContext.Current.Items["UltimasNotificaciones"] =
    generales
        .Concat(personalizadas)
        .OrderByDescending(n => n.fechaEnvio)
        .Take(5)
        .Select(n => new NotificacionResumenViewModel
        {
            titulo = n.titulo,
            fecha = n.fechaEnvio.Value.ToString("dd-MM-yyyy HH:mm")
        })
        .ToList();
            }
        }

        protected abstract string GetCurrentModule();
    }
}