using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Configuracion
{
    public class Tramite : CamposRegistroBase
    {

        public string Nombre { get; set; }
        public Proceso Proceso { get; set; }
        public long IdProceso { get; set; }
        public bool Activo { get; set; }
        public bool RequiereExpediente { get; set; }
        public string CorreoExpediente { get; set; }

        public AreaAtencion AreaAtencionProceso { get; set; }
        public long IdAreaAtencionProceso { get; set; }

    }
}
