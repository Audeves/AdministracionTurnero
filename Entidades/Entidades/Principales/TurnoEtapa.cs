using Entidades.Entidades.Administracion;
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
    public class TurnoEtapa : EntityBase
    {

        public Turno Turno { get; set; }
        public long IdTurno { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public EstatusTurnoEtapa Estatus { get; set; }
        public string EstatusDescr { get; set; }
        public ModuloAtencion ModuloAtencion { get; set; }
        public long IdModuloAtencion { get; set; }
        public Usuario UsuarioAtencion { get; set; }
        public long IdUsuarioAtencion { get; set; }

    }
}
