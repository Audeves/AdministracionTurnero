using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Configuracion
{
    public class ModuloAtencionGRIDDTO
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string AreaAtencion { get; set; }
        public List<Usuario> UsuariosAtencion { get; set; }
        public bool Activo { get; set; }
        public string ActivoStr { get { return this.Activo ? "Activo" : "Inactivo"; } }

        public string UsuariosAtencionStr { get { return string.Join(", ", this.UsuariosAtencion.Select(ua => ua.Nombre)); } }

    }
}
