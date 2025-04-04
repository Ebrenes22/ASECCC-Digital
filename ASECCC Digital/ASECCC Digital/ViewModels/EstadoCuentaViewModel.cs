using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.ViewModels
{
    public class EstadoCuentaViewModel
    {
        public int? UsuarioIdSeleccionado { get; set; }
        public Usuario UsuarioSeleccionado { get; set; }
        public List<SelectListItem> Usuario { get; set; }
        public List<Ahorros> Ahorros { get; set; }
        public List<Aportes> Aportes { get; set; }
        public List<Prestamos> Prestamos { get; set; }
        public List<BeneficiosServiciosCuenta> Beneficios { get; set; }
    }
}