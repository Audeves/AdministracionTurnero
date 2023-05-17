using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Principales
{
    public class Turno : EntityBase
    {

        public AreaAtencion AreaAtencion { get; set; }
        public long IdAreaAtencion { get; set; }
        public Proceso Proceso { get; set; }
        public long IdProceso { get; set; }
        public Tramite Tramite { get; set; }
        public long IdTramite { get; set; }
        public DateTime FechaEmisionTurno { get; set; }
        public int NumeroTurno { get; set; }
        public ConfiguracionTurnero ConfiguracionTurnero { get; set; }
        public long IdConfiguracionTurnero { get; set; }
        public string IdAlumno { get; set; }
        public string NombreCliente { get; set; }

        public EstatusTurnoEtapa Estatus { get; set; }

        public List<TurnoEtapa> Etapas { get; set; }

        public Turno()
        {
            Etapas = new List<TurnoEtapa>();
        }

    }
}
