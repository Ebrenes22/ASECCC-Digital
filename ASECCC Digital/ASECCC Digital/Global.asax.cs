using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ASECCC_Digital
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            // Excluir la ruta de login del proceso de autenticación
            var request = HttpContext.Current.Request;
            if (request.Url.AbsolutePath.ToLower().Contains("/Usuarios/Login") ||
                request.Url.AbsolutePath.ToLower().Contains("/Login"))
            {
                return; // No procesar autenticación para la ruta de login
            }

            // Procesar la autenticación para otras rutas
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    // userData = rol
                    string roles = authTicket.UserData;
                    var identity = new System.Security.Principal.GenericIdentity(authTicket.Name);
                    var principal = new System.Security.Principal.GenericPrincipal(identity, new[] { roles });
                    HttpContext.Current.User = principal;
                }
            }
        }
    }
}
