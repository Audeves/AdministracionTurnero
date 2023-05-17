using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Configuracion
{
    public class ConfiguracionTurneroGRIDDTO 
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string IP { get; set; }
        public string Campus { get; set; }
        public bool SolicitarId { get; set; }
        public bool SolicitarNombre { get; set; }

        public string SolicitarIdStr { get { return this.SolicitarId ? "Si" : "No"; } }
        public string SolicitarNombreStr { get { return this.SolicitarNombre ? "Si" : "No"; } }

        public List<AreaAtencion> AreasAtencion { get; set; }

        public string AreasAtencionStr { get { return string.Join(", ", this.AreasAtencion.Select(a => a.Nombre)); } }

    }
}
