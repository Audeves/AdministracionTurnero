using Entidades.Entidades.Configuracion;
using Entidades.Entidades.Principales;
using Entidades.Enumeradores;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Principales
{
    public class TurnoGRIDDTO
    {
        public long Id { get; set; }
        public AreaAtencion AreaAtencion { get; set; }
        public long IdAreaAtencion { get; set; }
        public Proceso Proceso { get; set; }
        public long IdProceso { get; set; }
        public Tramite Tramite { get; set; }
        public string NombreTramite { get; set; }
        public long IdTramite { get; set; }
        public DateTime FechaEmisionTurno { get; set; }
        public int NumeroTurno { get; set; }
        public ConfiguracionTurnero ConfiguracionTurnero { get; set; }
        public long IdConfiguracionTurnero { get; set; }
        public string IdAlumno { get; set; }
        public string NombreCliente { get; set; }

        public EstatusTurnoEtapa Estatus { get; set; }

        public List<TurnoEtapa> Etapas { get; set; }

    }
}
