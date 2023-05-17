using Entidades.Entidades.Configuracion;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Administracion
{
    public class Usuario : SQLEntityBase
    {

        public Usuario()
        {
            Perfiles = new List<Perfil>();
            AreasAtencionResponsable = new List<AreaAtencion>();

            ModulosAtencion = new List<ModuloAtencion>();
        }

        public string Emplid { get; set; }
        public string Nombre { get; set; }
        public string CuentaDominio { get; set; }
        public string DptoAdscripcion { get; set; }
        public string ClaveDptoAdscripcion { get; set; }
        public string DireccionAcademica { get; set; }
        public string ClaveDireccionAcademica { get; set; }
        public bool Activo { get; set; }
        public List<Perfil> Perfiles { get; set; }

        public bool Desarrollador { get; set; }

        public List<AreaAtencion> AreasAtencionResponsable { get; set; }

        public List<ModuloAtencion> ModulosAtencion { get; set; }


        public string Correo { get; set; }

    }
}
