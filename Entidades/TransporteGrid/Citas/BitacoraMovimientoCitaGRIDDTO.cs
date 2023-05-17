using Entidades.Enumeradores;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Citas
{
    public class BitacoraMovimientoCitaGRIDDTO : EntityBase
    {

        public long IdCita { get; set; } //columna id de tbl_citastramite

        public DateTime FechaEmision { get; set; }
        public string FechaEmisionStr { get { return this.FechaEmision.ToString("dd/MM/yyyy, HH:mm"); } }
        public string Usuario { get; set; } //cuenta de dominio

        public EstatusSolicitudCita Estatus { get; set; }
        public string EstatusStr { get; set; }
        public string ComentarioAdministrador { get; set; }

        public bool Asistio { get; set; }
        public string AsistioStr { get { return this.Asistio ? "Si" : "No"; } }

        public TipoMovimientoCita TipoMovimiento { get; set; }
        public string TipoMovimientoStr { get; set; }


        //Cita
        public string Tramite { get; set; }
        public string NombreSolicitante { get; set; }
        public string EmplId { get; set; }
        public DateTime FechaCita { get; set; }
        public string FechaCitaStr { get { return $"{this.FechaCita.ToString("dd/MM/yyyy")}, {this.HoraCita}"; } }
        public string HoraCita { get; set; }
        public DateTime FechaCaptura { get; set; }
        public string FechaCapturaStr { get { return this.FechaCaptura.ToString("dd/MM/yyyy, HH:mm"); } }
        public string Folio { get; set; }

    }
}
