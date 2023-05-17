using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Citas
{
    public class HistorialCancelacionMasiva : EntityBase
    {

        public DateTime FechaEmision { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool Pendientes { get; set; }
        public bool Agendadas { get; set; }
        public bool EnviarCorreo { get; set; }
        public string ComentarioCancelacion { get; set; }
        public int CantidadCitas { get; set; }

    }
}
