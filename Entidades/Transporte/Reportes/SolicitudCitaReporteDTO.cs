using Entidades.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Utils;

namespace Entidades.Transporte.Reportes
{
    public class SolicitudCitaReporteDTO
    {

        public long Id { get; set; }
        public long IdCita { get; set; } //???ke es ezto???

        public EstatusSolicitudCita Estatus { get; set; }
        public string EstatusStr { get { return this.Estatus.GetDescriptionEnum(); } }
        public bool Asistio { get; set; }
        public string AsistioStr { get { return this.Asistio ? "Si" : "No"; } }

        public DateTime FechaCita { get; set; }
        public string FechaCitaStr { get { return $"{this.FechaCita.ToString("dd/MM/yyyy")}, {this.HoraCita}"; } }
        public string HoraCita { get; set; }

        public DateTime FechaCaptura { get; set; }
        public string FechaCapturaStr { get { return this.FechaCaptura.ToString("dd/MM/yyyy, HH:mm"); } }

        public string TramiteActual { get; set; }
        public string NombreSolicitante { get; set; }

        public string AreaAtencion { get; set; }

        public string TramiteCapturado { get; set; }
        public string EmplId { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }


        public string ComentariosAdicionales { get; set; }
        public string PersonaAutorizada { get; set; }
        public string Parentezco { get; set; }
        public string Folio { get; set; }

        public string ComentariosAdministrador { get; set; }

    }
}
