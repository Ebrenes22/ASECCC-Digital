using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASECCC_Digital.ViewModels
{
    public class AhorroViewModel
    {
        public int AhorroId { get; set; }
        public string TipoAhorro { get; set; }
        public decimal MontoActual { get; set; }
        public DateTime FechaFin { get; set; }

        public DateTime FechaInicio { get; set; }
        public int? Plazo { get; set; }
        public string Estado { get; set; }
    }
}