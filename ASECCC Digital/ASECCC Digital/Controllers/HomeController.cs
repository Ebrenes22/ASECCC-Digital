using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class HomeController : BaseController
    {

        //--------VISTAS ADMIN--------------//
        [Authorize]
        public ActionResult Index()
        {

            return View();
        }

        protected override string GetCurrentModule()
        {
            return "Home";
        }

        //--------VISTAS ASOCIADO-------------//
    }
}