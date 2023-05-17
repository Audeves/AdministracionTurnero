using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Administracion
{
    public class PerfilGRIDDTO
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public string ActivoStr { get { return this.Activo ? "Activo" : "Inactivo"; } }

        public List<AreaAtencion> AreasAtencion { get; set; }

        public PerfilGRIDDTO()
        {
            AreasAtencion = new List<AreaAtencion>();
        }

    }
}
