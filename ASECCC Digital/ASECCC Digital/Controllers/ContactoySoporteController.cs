using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class ContactoySoporteController : Controller
    {


        //--------VISTAS ASOCIADO-------------//
        [HttpGet]
        public ActionResult FAQ()
        {
            return View();
        }
    }
}