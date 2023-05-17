using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Configuracion
{
    public class AreaAtencionGRIDDTO 
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Campus { get; set; }
        public bool Activo { get; set; }
        public string ActivoStr { get { return this.Activo ? "Activo" : "Inactivo"; } }
        public List<Usuario> Responsables { get; set; }

        public string ResponsablesStr { get { return this.Responsables == null ? "" : string.Join(", ", this.Responsables.Select(r => r.Nombre)); } }

        public bool DisponibleCitas { get; set; }
        public string DisponibleCitasStr { get { return this.DisponibleCitas ? "Si" : ""; } }

    }
}
