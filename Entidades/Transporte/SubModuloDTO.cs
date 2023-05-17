using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Transporte
{
    public class SubModuloDTO
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string ClaseIcono { get; set; }
        public bool Activo { get; set; }
        public string ActivoStr { get { return this.Activo ? "Activo" : "Inactivo"; } }

        public string NombreModulo { get; set; }
        public bool ActivoModulo { get; set; }
        public string ActivoModuloStr { get { return this.ActivoModulo ? "Activo" : "Inactivo"; } }

    }
}
