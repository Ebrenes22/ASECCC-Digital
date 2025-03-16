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
    }
}