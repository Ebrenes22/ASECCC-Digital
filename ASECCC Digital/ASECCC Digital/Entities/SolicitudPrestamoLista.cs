using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASECCC_Digital.Entities
{
    public class SolicitudPrestamoLista
    {
        public List<SolicitudesPrestamo> Pendientes { get; set; }
        public List<SolicitudesPrestamo> EnRevision { get; set; }
        public List<SolicitudesPrestamo> Aprobadas { get; set; }
        public List<SolicitudesPrestamo> Rechazadas { get; set; }
    }
}