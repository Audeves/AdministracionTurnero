using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Principales
{
    public class ModuloAtencionBitacora : EntityBase
    {

        public ModuloAtencion ModuloAtencion { get; set; }
        public long IdModuloAtencion { get; set; }
        public Usuario Usuario { get; set; }
        public long IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public bool Activo { get; set; }

    }
}
