using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class HomeController : Controller
    {

        //--------VISTAS ADMIN--------------//
        [Authorize(Roles = "asociado")]
        public ActionResult Index()
        {
            Session["Rol"] = "Admin";
            Session["FechaIngreso"] = "15-01-2012";
            return View();
        }

        //--------VISTAS ASOCIADO-------------//
    }
}