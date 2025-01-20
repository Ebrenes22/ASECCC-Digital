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
        public ActionResult Index()
        {
            Session["Rol"] = "Admin";
            return View();
        }

        //--------VISTAS ASOCIADO-------------//
    }
}