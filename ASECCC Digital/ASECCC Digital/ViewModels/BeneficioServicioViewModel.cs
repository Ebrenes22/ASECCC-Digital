using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASECCC_Digital.ViewModels
{
    public class BeneficioServicioViewModel
    {
        public BeneficioServicio BeneficioServicio { get; set; }

        public IEnumerable<BeneficioServicio> BeneficioServicios { get; set; }

        public BeneficioServicioCuenta BeneficioServicioCuenta { get; set; }

        public IEnumerable<BeneficioServicioCuenta> BeneficioServicioCuentas { get; set; }

        public BeneficioTransaccion BeneficioTransaccion { get; set; }

        public IEnumerable<BeneficioTransaccion> BeneficioTransacciones { get; set; }

        public Usuario Usuario { get; set; }

        public List<Usuario> Usuarios { get; set; } = new List<Usuario>(); // 🔒 Blindaje
    }
}
