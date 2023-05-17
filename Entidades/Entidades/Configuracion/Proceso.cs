using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Configuracion
{
    public class Proceso : CamposRegistroBase
    {

        public string Nombre { get; set; }
        public AreaAtencion AreaAtencion { get; set; }
        public long IdAreaAtencion { get; set; }
        public bool Activo { get; set; }

    }
}
