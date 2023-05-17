using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Configuracion
{
    public class ConfiguracionTurnero : EntityBase
    {

        public string Nombre { get; set; }
        public string IP { get; set; }
        public string CampusPS { get; set; }
        public string CampusDescr { get; set; }
        public bool SolicitarId { get; set; }
        public bool SolicitarNombre { get; set; }
        public bool Activo { get; set; }

        public List<AreaAtencion> AreasAtencion { get; set; }


        public ConfiguracionTurnero()
        {
            AreasAtencion = new List<AreaAtencion>();
        }

    }
}
