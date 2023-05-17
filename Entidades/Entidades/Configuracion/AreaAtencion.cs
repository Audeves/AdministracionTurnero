using Entidades.Entidades.Administracion;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Configuracion
{
    public class AreaAtencion : EntityBase
    {

        public string Nombre { get; set; }
        public string CampusPS { get; set; }
        public string CampusDescr { get; set; }
        public bool Activo { get; set; }
        public string Ticket { get; set; }
        public string Ayuda { get; set; }

        public List<Usuario> Responsables { get; set; }

        public List<Perfil> Perfiles { get; set; }

        public AreaAtencion()
        {
            Responsables = new List<Usuario>();
            Perfiles = new List<Perfil>();

            ModulosAtencion = new List<ModuloAtencion>();
        }

        public ConfiguracionTurnero ConfiguracionTurnero { get; set; }
        public long? IdConfiguracionTurnero { get; set; }

        public ConfiguracionPantalla ConfiguracionPantalla { get; set; }
        public long? IdConfiguracionPantalla { get; set; }


        public List<ModuloAtencion> ModulosAtencion { get; set; }

        public bool? DisponibleCitas { get; set; }
        public string ResponsablePermisoCitas { get; set; }
        public string LugarCitas { get; set; }

        public string CorreoAgendada { get; set; }
        public string CorreoCancelada { get; set; }
        public string CorreoRemitenteCitas { get; set; }

    }
}
