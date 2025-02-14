using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

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
            string path = request.Url.AbsolutePath.ToLower();

            if (path.Contains("/usuarios/login") || path.Contains("/login"))
            {
                return; // No procesar autenticación para la ruta de login
            }

            // Obtener la cookie de autenticación
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                try
                {
                    // Desencriptar el ticket de autenticación
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null && !authTicket.Expired)
                    {
                        // Crear FormsIdentity en lugar de GenericIdentity
                        var identity = new FormsIdentity(authTicket);
                        string roles = authTicket.UserData.ToLower(); // Mantener los roles en minúsculas

                        var principal = new System.Security.Principal.GenericPrincipal(identity, new[] { roles });
                        HttpContext.Current.User = principal;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en PostAuthenticateRequest: " + ex.Message);
                }
            }
        }


    }
}

