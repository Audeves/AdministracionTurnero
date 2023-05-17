using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Configuracion
{
    public class ProcesoGRIDDTO
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string AreaAtencion { get; set; }
        public bool Activo { get; set; }
        public string ActivoStr { get { return this.Activo ? "Activo" : "Inactivo"; } }

    }
}
