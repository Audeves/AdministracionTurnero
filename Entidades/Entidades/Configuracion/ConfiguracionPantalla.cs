using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Configuracion
{
    public class ConfiguracionPantalla : EntityBase
    {

        public string IP { get; set; }
        public string CampusPS { get; set; }
        public string CampusDescr { get; set; }
        public bool Activo { get; set; }

        public List<AreaAtencion> AreasAtencion { get; set; }

        public ConfiguracionPantalla()
        {
            AreasAtencion = new List<AreaAtencion>();
        }

    }
}
