using Entidades.Entidades.Administracion;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Configuracion
{
    public class ModuloAtencion : EntityBase
    {

        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public AreaAtencion AreaAtencion { get; set; }
        public long IdAreaAtencion { get; set; }
        public List<Proceso> Procesos { get; set; }
        public List<Usuario> UsuariosAtencion { get; set; }

        public ModuloAtencion()
        {
            Procesos = new List<Proceso>();
            UsuariosAtencion = new List<Usuario>();
        }

    }
}
