using Entidades.Entidades.Configuracion;
using Entidades.Entidades.General;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Administracion
{
    public class Perfil : CamposRegistroBase
    {

        public Perfil()
        {
            Pantallas = new List<Pantalla>();
            AreasAtencion = new List<AreaAtencion>();
        }

        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<Pantalla> Pantallas { get; set; }
        public bool Desarrollador { get; set; }

        public bool ElegibleAsignar { get; set; }

        public List<AreaAtencion> AreasAtencion { get; set; }

    }
}
