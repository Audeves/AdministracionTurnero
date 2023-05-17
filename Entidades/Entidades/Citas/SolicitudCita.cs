using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Citas
{
    public class SolicitudCita : EntityBase
    {

        public long IdCita { get; set; } //???ke es ezto???

        public EstatusSolicitudCita Estatus { get; set; }
        public bool Asistio { get; set; }

        public DateTime FechaCita { get; set; }
        public string HoraCita { get; set; }
        public DateTime FechaCaptura { get; set; }
        public TramiteCita Tramite { get; set; }
        public long IdTramite { get; set; }
        public string NombreSolicitante { get; set; }
        public string Email { get; set; }

        public AreaAtencion AreaAtencion { get; set; }
        public long IdAreaAtencion { get; set; }

        public string TramiteStr { get; set; }
        public string ComentariosAdicionales { get; set; }
        public string EmplId { get; set; }
        public string Telefono { get; set; }

        public string PersonaAutorizada { get; set; }
        public string Parentezco { get; set; }
        public string Folio { get; set; }

        public string ComentariosAdministrador { get; set; }

        public List<BitacoraMovimientoCita> BitacoraMovimientos { get; set; }

        public SolicitudCita()
        {
            BitacoraMovimientos = new List<BitacoraMovimientoCita>();
        }
    }
}
