using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class UsuariosController : Controller
    {
        //--------VISTAS ADMIN--------------//
        // GET: Usuario
        public ActionResult Usuario()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        //--------VISTAS USUARIO--------------//
    }
}