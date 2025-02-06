using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASECCC_Digital.Models
{

    public class AhorroModel
    {
        public int AhorroId { get; set; }
        public decimal MontoActual { get; set; }
        public DateTime? FechaInicio { get; set; }
        public string Estado { get; set; }
        public string TipoAhorro { get; set; }
        public int? Plazo { get; set; }

        public string MontoFormateado
        {
            get { return MontoActual.ToString("C"); }
        }
    }
}
