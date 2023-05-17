using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Configuracion
{
    public class ConfiguracionPantallaGRIDDTO
    {

        public long Id { get; set; }
        public string IP { get; set; }
        public string Campus { get; set; }
        
        public List<AreaAtencion> AreasAtencion { get; set; }

        public string AreasAtencionStr { get { return string.Join(", ", this.AreasAtencion.Select(a => a.Nombre)); } }

    }
}
